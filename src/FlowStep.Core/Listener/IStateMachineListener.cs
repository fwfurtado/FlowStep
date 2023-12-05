using FlowStep.Core.Context;
using FlowStep.Core.State;
using FlowStep.Core.Support;
using FlowStep.Core.Transition;

namespace FlowStep.Core.Listener;

public interface IStateMachineListener<TState, TEvent>
{
    void StateChanged(IState<TState, TEvent> from, IState<TState, TEvent> to);


    void StateEntered(IState<TState, TEvent> state);


    void StateExited(IState<TState, TEvent> state);


    void EventNotAccepted(IMessage<TEvent> @event);


    void Transition(ITransition<TState, TEvent> transition);


    void TransitionStarted(ITransition<TState, TEvent> transition);


    void TransitionEnded(ITransition<TState, TEvent> transition);


    void StateMachineStarted(IStateMachine<TState, TEvent> stateMachine);


    void StateMachineStopped(IStateMachine<TState, TEvent> stateMachine);


    void StateMachineError(IStateMachine<TState, TEvent> stateMachine, Exception? exception);


    void ExtendedStateChanged(object key, object value);

    void StateContext(IStateContext<TState, TEvent> context);
}