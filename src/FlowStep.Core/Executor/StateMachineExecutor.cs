using FlowStep.Core.Context;
using FlowStep.Core.Default;
using FlowStep.Core.Interceptors;
using FlowStep.Core.State;
using FlowStep.Core.State.Pseudo;
using FlowStep.Core.Support;
using FlowStep.Core.Support.Extensions;
using FlowStep.Core.Transition;

namespace FlowStep.Core.Executor;

public class StateMachineExecutor<TState, TEvent>(
    IStateMachine<TState, TEvent> stateMachine,
    IStateMachine<TState, TEvent> relayMachine,
    IEnumerable<ITransition<TState, TEvent>> transitions,
    IMessage<TEvent>? forwardInitialEvent = null
) : IStateMachineExecutor<TState, TEvent>
{
    private readonly CompositeStateMachineInterceptor<TState, TEvent> _interceptors = new();
    private readonly List<ITransition<TState, TEvent>> _triggerLessTransitions = new();
    private readonly Queue<IMessage<TEvent>> _eventQueue = new();

    public bool IsRunning { get; private set; }
    public bool IsInitialEnabled { get; }
    public IMessage<TEvent>? ForwardInitialEvent { get; } = forwardInitialEvent;
    public IStateMachineExecutorTransit<TState, TEvent>? ExecutorTransit { get; }

    public void Start()
    {
        IsRunning = true;
    }

    public void Stop()
    {
        IsRunning = false;
    }

    public IStateMachineExecutorResult EnqueueEvent(IMessage<TEvent> message)
    {
        _eventQueue.Enqueue(message);

        try
        {
            while (_eventQueue.Count != 0)
            {
                return HandleEvent(_eventQueue.Dequeue());
            }
        }
        catch (Exception e)
        {
            return new IncompleteErrorResult(e);
        }

        return new CompleteErrorResult();
    }


    private IStateMachineExecutorResult HandleEvent(IMessage<TEvent> message)
    {
        var currentState = stateMachine.State;

        var triggerContext = new DefaultTriggerContext<TState, TEvent>(message.Payload);

        foreach (var transition in transitions)
        {
            if (transition.Trigger is null) continue;

            if (currentState.Ids.ContainsNone(transition.Source.Ids)) continue;

            if (transition.Trigger.Evaluate(triggerContext))
            {
                return new SuccessResult();
            }
        }

        return new CompleteErrorResult();
    }

    public void ExecuteTriggerLessTransition(IStateContext<TState, TEvent> context, IState<TState, TEvent> state)
    {
        foreach (var transition in _triggerLessTransitions)
        {
            var currentState = stateMachine.State;
            var source = transition.Source;
            
            if(currentState.Ids.ContainsNone(source.Ids)) continue;

            if (source is { IsOrthogonal: true }) continue;
            
            if (source.States.Where(s => s != source).Any(s => s == state)) continue;

            var target = transition.Target;

            if (target is { Pseudo.Kind: PseudoStateKind.Join })
            {
             // TODO: Implement Join   
            }
            
        }
    }

    public void AddStateMachineInterceptor(IStateMachineInterceptor<TState, TEvent> interceptor)
    {
        _interceptors.Add(interceptor);
    }
}

internal class SuccessResult : IStateMachineExecutorResult
{
    public bool IsCompleted => true;
    public Exception? Error => null;
}

internal class IncompleteErrorResult(Exception exception) : IStateMachineExecutorResult
{
    public bool IsCompleted => false;
    public Exception Error { get; } = exception;
}

internal class CompleteErrorResult : IStateMachineExecutorResult
{
    public bool IsCompleted => true;
    public Exception Error { get; } = new("No transition found");
}