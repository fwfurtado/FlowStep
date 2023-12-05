using FlowStep.Core.Access;
using FlowStep.Core.Context;
using FlowStep.Core.Default;
using FlowStep.Core.Executor;
using FlowStep.Core.Listener;
using FlowStep.Core.State;
using FlowStep.Core.State.Extended;
using FlowStep.Core.State.Pseudo;
using FlowStep.Core.Support;
using FlowStep.Core.Support.Extensions;
using FlowStep.Core.Transition;
using FlowStep.Core.Trigger;

namespace FlowStep.Core;

public partial class AbstractStateMachine<TState, TEvent>
(
    IState<TState, TEvent> initial,
    IEnumerable<IState<TState, TEvent>> states,
    IEnumerable<ITransition<TState, TEvent>> transitions,
    ITransition<TState, TEvent>? initialTransition = null,
    Guid? guid = null,
    IExtendedState? extendedState = null,
    IMessage<TEvent>? initialEvent = null
) : IStateMachine<TState, TEvent> where TState : notnull
{
    private IStateMachineExecutor<TState, TEvent> _stateMachineExecutor = null!;

    private Lazy<IExtendedState>? _lazyExtendedState;

    private IState<TState, TEvent>? _lastState;
    private IState<TState, TEvent>? _currentState;

    public Guid Uuid { get; } = guid ?? Guid.NewGuid();
    public string Id { get; set; } = string.Empty;

    public IEnumerable<IState<TState, TEvent>> States { get; } = states;
    public IEnumerable<ITransition<TState, TEvent>> Transitions { get; } = transitions;

    public IState<TState, TEvent> Initial { get; } = initial;

    public IExtendedState ExtendedState => ComputedExtendedState();

    private IExtendedState ComputedExtendedState()
    {
        _lazyExtendedState ??= new Lazy<IExtendedState>(() =>
        {
            if (extendedState is not null) return extendedState;

            var listener = new ExtendedStateListenerByAction(
                (key, value) =>
                    NotifyExtendedStateChanged(key, value, BuildStateContext(Stage.ExtendedStateChanged, Relay))
            );

            return new DefaultExtendedState(listener: listener);
        });


        return _lazyExtendedState.Value;
    }

    public ITransition<TState, TEvent> InitialTransition { get; } =
        initialTransition ?? new InitialTransition<TState, TEvent>(initial);

    public IPseudoState<TState, TEvent>? History { get; set; }

    public Exception? Error { get; private set; }

    private void ClearError()
    {
        Error = null;
    }

    private void SetError(Exception exception)
    {
        var error = Interceptors.Error(this, exception);
        Error = error;
    }

    public bool HasError => Error is not null;

    public bool IsComplete
    {
        get
        {
            if (_currentState is null) return !IsRunning;

            return _currentState.Pseudo?.Kind == PseudoStateKind.End;
        }
    }

    public IState<TState, TEvent> State
    {
        get
        {
            if (_lastState is not null && IsComplete)
            {
                return _lastState;
            }

            return _currentState ?? Initial;
        }
    }


    public IEnumerable<IStateMachineEventResult<TState, TEvent>> SendEvents(IEnumerable<IMessage<TEvent>> events)
    {
        return events.Select(Send);
    }

    public IEnumerable<IStateMachineEventResult<TState, TEvent>> SendEvent(IMessage<TEvent> @event)
    {
        yield return Send(@event);
    }

    private IStateMachineEventResult<TState, TEvent> Send(IMessage<TEvent> message)
    {
        if (HasError)
        {
            return new DefaultStateMachineEventResult<TState, TEvent>(this, message, ResultType.Denied);
        }

        var newMessage = Interceptors.Aggregate(message, (current, interceptor) => interceptor.PreEvent(current, this));


        var triggerContext = new DefaultTriggerContext<TState, TEvent>(newMessage.Payload);

        if (_currentState is null)
        {
            return new DefaultStateMachineEventResult<TState, TEvent>(this, newMessage, ResultType.Denied);
        }

        var sentResults = _currentState.Send(newMessage);

        if (sentResults.All(r => r.Result != ResultType.Accepted))
            return new DefaultStateMachineEventResult<TState, TEvent>(this, newMessage, ResultType.Denied);


        var hasEligibleTransition = Transitions
            .Where(t => t.Source.Ids.ContainsAny(_currentState.Ids))
            .Any(transition => transition.Trigger != null && transition.Trigger.Evaluate(triggerContext));

        if (hasEligibleTransition)
        {
            return _stateMachineExecutor.EnqueueEvent(newMessage) switch
            {
                { IsCompleted: true, Error: null } => new DefaultStateMachineEventResult<TState, TEvent>(
                    this,
                    newMessage,
                    ResultType.Accepted
                ),
                _ => new DefaultStateMachineEventResult<TState, TEvent>(
                    this,
                    newMessage,
                    ResultType.Denied
                )
            };
        }

        return new DefaultStateMachineEventResult<TState, TEvent>(this, newMessage, ResultType.Denied);
    }


    public void AddStateListener(IStateMachineListener<TState, TEvent> listener)
    {
        StateListener.Register(listener);
    }

    public void RemoveStateListener(IStateMachineListener<TState, TEvent> listener)
    {
        StateListener.Unregister(listener);
    }
}

public partial class AbstractStateMachine<TState, TEvent> : StateMachineObjectSupport<TState, TEvent>
    where TState : notnull
{
    private readonly Dictionary<ITrigger<TState, TEvent>, ITransition<TState, TEvent>> _triggerToTransition = new();
    private readonly List<ITransition<TState, TEvent>> _triggerlessTransitions = new();

    protected override void OnInit()
    {
        PopulateTransitions(transitions);
        SetupStates(states);
    }

    private void PopulateTransitions(IEnumerable<ITransition<TState, TEvent>> transitions)
    {
        foreach (var transition in transitions)
        {
            var trigger = transition.Trigger;

            if (trigger is null)
            {
                _triggerlessTransitions.Add(transition);
                continue;
            }

            _triggerToTransition.Add(trigger, transition);
        }
    }

    private void SetupStates(IEnumerable<IState<TState, TEvent>> states)
    {
        foreach (var state in states)
        {
            var thisMachine = this;

            var stateListener = new StateListenerByAction<TState, TEvent>(
                doOnComplete: ctx =>
                {
                    if (Relay is AbstractStateMachine<TState, TEvent> relayMachine)
                    {
                        relayMachine.ExecuteTriggerLessTransitions(thisMachine, ctx, state);
                    }
                }
            );

            state.AddStateListener(stateListener);

            if (state is AbstractState<TState, TEvent> abstractState)
            {
                if (state.IsSubMachineState)
                {
                    var subMachine = abstractState.SubMachine!;
                    subMachine?.AddStateListener(RelayStateListener);
                }
                else if (state.IsOrthogonal)
                {
                    foreach (var region in abstractState.Regions)
                    {
                        region.AddStateListener(RelayStateListener);
                    }
                }
            }

            if (state.Pseudo is { Kind: PseudoStateKind.HistoryDeep })
            {
                History = state.Pseudo;
            }
        }
    }

    private void ExecuteTriggerLessTransitions(
        IStateMachine<TState, TEvent> machine,
        IStateContext<TState, TEvent> context,
        IState<TState, TEvent> state
    )
    {
        _stateMachineExecutor.ExecuteTriggerLessTransition(context, state);

        if (_currentState is not AbstractState<TState, TEvent> currentState) return;

        switch (currentState)
        {
            case { IsOrthogonal: true }:
            {
                foreach (var region in currentState.Regions)
                {
                    if (region is AbstractStateMachine<TState, TEvent> regionMachine)
                    {
                        regionMachine.ExecuteTriggerLessTransitions(machine, context, regionMachine.State);
                    }
                }

                break;
            }
            case { IsSubMachineState: true, SubMachine: AbstractStateMachine<TState, TEvent> subMachine }:
                subMachine.ExecuteTriggerLessTransitions(machine, context, subMachine.State);
                break;
        }
    }


    private DefaultStateContext<TState, TEvent> BuildStateContext(
        Stage stage,
        IStateMachine<TState, TEvent> machine,
        IMessage<TEvent>? message = null,
        ITransition<TState, TEvent>? transition = null,
        IState<TState, TEvent>? source = null,
        IState<TState, TEvent>? target = null,
        IEnumerable<IState<TState, TEvent>>? sources = null,
        IEnumerable<IState<TState, TEvent>>? targets = null,
        Exception? exception = null
    )
    {
        var headers = message?.Headers;

        return new DefaultStateContext<TState, TEvent>(
            stage,
            machine,
            message,
            headers,
            transition,
            ExtendedState,
            source,
            target,
            sources,
            targets,
            exception
        );
    }
}

public partial class AbstractStateMachine<TState, TEvent> : IStateMachineAccess<TState, TEvent> where TState : notnull
{
    private IStateMachine<TState, TEvent>? _relay = null;

    public IStateMachine<TState, TEvent> Relay
    {
        get => _relay ?? this;

        set => _relay = value;
    }

    public IStateMachine<TState, TEvent>? Parent { get; }
    public IMessage<TEvent> ForwardedInitialEvent { get; }

    public bool? InitialEnabled { get; }


    public void AddStateMachineInterceptor(IStateMachineInterceptor<TState, TEvent> interceptor)
    {
    }
}