using FlowStep.Core.Context;
using FlowStep.Core.State;
using FlowStep.Core.Support;
using FlowStep.Core.Transition;

namespace FlowStep.Core.Executor;

public interface IStateMachineExecutor<TState, TEvent> : IStateMachineLifecycle
{
    IStateMachineExecutorResult EnqueueEvent(IMessage<TEvent> message);

    void ExecuteTriggerLessTransition(IStateContext<TState, TEvent> context, IState<TState, TEvent> state);

    bool IsInitialEnabled { get; }

    IMessage<TEvent>? ForwardInitialEvent { get; }

    IStateMachineExecutorTransit<TState, TEvent>? ExecutorTransit { get; }

    void AddStateMachineInterceptor(IStateMachineInterceptor<TState, TEvent> interceptor);
}

public interface IStateMachineExecutorTransit<TState, TEvent>
{
    void Transit(
        ITransition<TState, TEvent> transition,
        IStateContext<TState, TEvent> context,
        IMessage<TEvent> message
    );
}

public interface IStateMachineExecutorResult
{
    public bool IsCompleted { get; }
    public Exception? Error { get; }
}