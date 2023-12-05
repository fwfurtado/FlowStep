using FlowStep.Core.Context;
using FlowStep.Core.State;
using FlowStep.Core.Support;
using FlowStep.Core.Transition;

namespace FlowStep.Core.Listener;

public class NoOpStateMachineListener<TState, TEvent> : IStateMachineListener<TState, TEvent>
{
    public virtual void StateChanged(IState<TState, TEvent> from, IState<TState, TEvent> to)
    {
    }

    public virtual void StateEntered(IState<TState, TEvent> state)
    {
    }

    public virtual void StateExited(IState<TState, TEvent> state)
    {
    }

    public virtual void EventNotAccepted(IMessage<TEvent> @event)
    {
    }

    public virtual void Transition(ITransition<TState, TEvent> transition)
    {
    }

    public virtual void TransitionStarted(ITransition<TState, TEvent> transition)
    {
    }

    public virtual void TransitionEnded(ITransition<TState, TEvent> transition)
    {
    }

    public virtual void StateMachineStarted(IStateMachine<TState, TEvent> stateMachine)
    {
    }

    public virtual void StateMachineStopped(IStateMachine<TState, TEvent> stateMachine)
    {
    }

    public virtual void StateMachineError(IStateMachine<TState, TEvent> stateMachine, Exception? exception)
    {
    }

    public void ExtendedStateChanged(object key, object value)
    {
        
    }

    public virtual void StateContext(IStateContext<TState, TEvent> context)
    {
    }
}