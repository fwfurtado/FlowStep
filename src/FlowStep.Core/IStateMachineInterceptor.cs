using FlowStep.Core.Context;
using FlowStep.Core.State;
using FlowStep.Core.Support;
using FlowStep.Core.Transition;

namespace FlowStep.Core;

public interface IStateMachineInterceptor<TState, TEvent>
{
    IMessage<TEvent> PreEvent(IMessage<TEvent> message, IStateMachine<TState, TEvent> machine);

    void PreStateChange(
        IState<TState, TEvent> state,
        IMessage<TEvent> message,
        ITransition<TState, TEvent> transition,
        IStateMachine<TState, TEvent> machine,
        IStateMachine<TState, TEvent> rootMachine
    );

    void PostStateChange(
        IState<TState, TEvent> state,
        IMessage<TEvent> message,
        ITransition<TState, TEvent> transition,
        IStateMachine<TState, TEvent> machine,
        IStateMachine<TState, TEvent> rootMachine
    );

    IStateContext<TState, TEvent> PreTransition(IStateContext<TState, TEvent> context);
    IStateContext<TState, TEvent> PostTransition(IStateContext<TState, TEvent> context);

    Exception Error(IStateMachine<TState, TEvent> machine, Exception exception);
}