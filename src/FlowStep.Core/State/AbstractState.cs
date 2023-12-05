using System.Collections.Concurrent;
using FlowStep.Core.Action;
using FlowStep.Core.Context;
using FlowStep.Core.Listener;
using FlowStep.Core.Region;
using FlowStep.Core.State.Pseudo;
using FlowStep.Core.Support;
using FlowStep.Core.Support.Extensions;

namespace FlowStep.Core.State;

public abstract class AbstractState<TState, TEvent>(
    TState id,
    IPseudoState<TState, TEvent>? pseudoState = null,
    IEnumerable<StateContextAction<TState, TEvent>>? entryActions = null,
    IEnumerable<StateContextAction<TState, TEvent>>? stateActions = null,
    IEnumerable<StateContextAction<TState, TEvent>>? exitActions = null,
    IEnumerable<IRegion<TState, TEvent>>? regions = null,
    IStateMachine<TState, TEvent>? subStateMachine = null
) : IState<TState, TEvent>
{
    private readonly List<IRegion<TState, TEvent>> _regions = regions?.ToList() ?? new List<IRegion<TState, TEvent>>();

    private readonly BlockingCollection<IStateMachineListener<TState, TEvent>> _compositeListeners = new();
    private readonly CompositeActionListener<TState, TEvent> _compositeActionListener = new();
    private readonly CompositeStateListener<TState, TEvent> _stateListener = new();

    public TState Id => id;
    public abstract IEnumerable<TState> Ids { get; }
    public abstract IEnumerable<IState<TState, TEvent>> States { get; }
    public IPseudoState<TState, TEvent>? Pseudo => pseudoState;

    public IEnumerable<StateContextAction<TState, TEvent>> EntryActions { get; } =
        entryActions ?? new List<StateContextAction<TState, TEvent>>();

    public IEnumerable<StateContextAction<TState, TEvent>> StateActions { get; } =
        stateActions ?? new List<StateContextAction<TState, TEvent>>();

    public IEnumerable<StateContextAction<TState, TEvent>> ExitActions { get; } =
        exitActions ?? new List<StateContextAction<TState, TEvent>>();

    public bool IsSimple => !IsComposite && !IsSubMachineState;
    public bool IsComposite => _regions.Count != 0;
    public bool IsOrthogonal => _regions.Count > 1;
    public bool IsSubMachineState => subStateMachine is not null;
    
    public IStateMachine<TState, TEvent>? SubMachine => subStateMachine;
    public IEnumerable<IRegion<TState, TEvent>> Regions => _regions;


    public virtual IEnumerable<IStateMachineEventResult<TState, TEvent>> Send(IMessage<TEvent> @event)
    {
        return Enumerable.Empty<IStateMachineEventResult<TState, TEvent>>();
    }

    public virtual void Entry(IStateContext<TState, TEvent> context)
    {
        if (subStateMachine is not null)
        {
            RegisterInCompletionListener(subStateMachine);
        }
        else if (_regions.Count != 0)
        {
            foreach (var region in _regions)
            {
                RegisterInCompletionListener(region);
            }
        }

        _stateListener.OnEntry(context);
    }

    private void RegisterInCompletionListener(IRegion<TState, TEvent> region)
    {
        var listener = new EntryStateListener<TState, TEvent>(region, _compositeListeners);

        _compositeListeners.Add(listener);
        region.AddStateListener(listener);
    }

    public virtual void Exit(IStateContext<TState, TEvent> context)
    {
        if (subStateMachine is not null)
        {
            foreach (var listener in _compositeListeners)
            {
                subStateMachine.RemoveStateListener(listener);
            }
        }

        else if (_regions.Count != 0)
        {
            foreach (var region in _regions)
            {
                foreach (var listener in _compositeListeners)
                {
                    region.RemoveStateListener(listener);
                }
            }
        }

        _compositeListeners.Clear();

        _stateListener.OnExit(context);
    }

    public void AddStateListener(IStateListener<TState, TEvent> listener)
    {
        _stateListener.Register(listener);
    }

    public void RemoveStateListener(IStateListener<TState, TEvent> listener)
    {
        _stateListener.Unregister(listener);
    }

    public void AddActionListener(IActionListener<TState, TEvent> listener)
    {
        lock (this)
        {
            _compositeActionListener.Register(listener);
        }
    }

    public void RemoveActionListener(IActionListener<TState, TEvent> listener)
    {
        lock (this)
        {
            _compositeActionListener.Unregister(listener);
        }
    }

    protected void ExecuteAction(StateContextAction<TState, TEvent> action, IStateContext<TState, TEvent> context)
    {
        var now = DateTime.UtcNow;

        action(context);

        var duration = DateTime.UtcNow - now;
        lock (this)
        {
            _compositeActionListener.OnExecute(context.StateMachine, action, duration.Milliseconds);
        }   
    }
}

internal class EntryStateListener<TState, TEvent>
(
    IRegion<TState, TEvent> region,
    BlockingCollection<IStateMachineListener<TState, TEvent>> compositeListeners
) : NoOpStateMachineListener<TState, TEvent>
{
    public override void StateContext(IStateContext<TState, TEvent> context)
    {
        if (context.Stage != Stage.StateMachineStop)
        {
            return;
        }


        if (context.StateMachine != region || !region.IsComplete) return;


        var exists = compositeListeners.Contains(this);

        if (exists)
        {
            compositeListeners.Remove(this);
        }

        region.RemoveStateListener(this);
    }
}