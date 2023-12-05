using FlowStep.Core.Context;
using FlowStep.Core.State;
using FlowStep.Core.Support;
using FlowStep.Core.Transition;

namespace FlowStep.Core.Listener;

public class CompositeStateMachineListener<TState, TEvent> :
    AbstractCompositeListener<IStateMachineListener<TState, TEvent>>, IStateMachineListener<TState, TEvent>
{
    public void StateChanged(IState<TState, TEvent> from, IState<TState, TEvent> to)
    {
        Notify(listener => listener.StateChanged(from, to));
    }

    public void StateEntered(IState<TState, TEvent> state)
    {
        Notify(listener => listener.StateEntered(state));
    }

    public void StateExited(IState<TState, TEvent> state)
    {
        Notify(listener => listener.StateExited(state));
    }

    public void EventNotAccepted(IMessage<TEvent> @event)
    {
        Notify(listener => listener.EventNotAccepted(@event));
    }

    public void Transition(ITransition<TState, TEvent> transition)
    {
        Notify(listener => listener.Transition(transition));
    }

    public void TransitionStarted(ITransition<TState, TEvent> transition)
    {
        Notify(listener => listener.TransitionStarted(transition));
    }

    public void TransitionEnded(ITransition<TState, TEvent> transition)
    {
        Notify(listener => listener.TransitionEnded(transition));
    }

    public void StateMachineStarted(IStateMachine<TState, TEvent> stateMachine)
    {
        Notify(listener => listener.StateMachineStarted(stateMachine));
    }

    public void StateMachineStopped(IStateMachine<TState, TEvent> stateMachine)
    {
        Notify(listener => listener.StateMachineStopped(stateMachine));
    }

    public void StateMachineError(IStateMachine<TState, TEvent> stateMachine, Exception? exception)
    {
        Notify(listener => listener.StateMachineError(stateMachine, exception));
    }

    public void ExtendedStateChanged(object key, object value)
    {
        Notify(listener => listener.ExtendedStateChanged(key, value));
    }

    public void StateContext(IStateContext<TState, TEvent> context)
    {
        Notify(listener => listener.StateContext(context));
    }


    private void Notify(Action<IStateMachineListener<TState, TEvent>> action)
    {
        foreach (var listener in Listeners.InReverseOrder)
        {
            action(listener);
        }
    }
}