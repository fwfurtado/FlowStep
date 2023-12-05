using FlowStep.Core.Context;
using FlowStep.Core.Interceptors;
using FlowStep.Core.Listener;
using FlowStep.Core.State;
using FlowStep.Core.Transition;

namespace FlowStep.Core.Support;

public abstract class StateMachineObjectSupport<TState, TEvent> : IStateMachineLifecycle
{
    public bool IsRunning { get; private set; }

    private IStateMachineListener<TState, TEvent>? _relayListener;
    protected CompositeStateMachineListener<TState, TEvent> StateListener { get; } = new();
    protected CompositeStateMachineInterceptor<TState, TEvent> Interceptors { get; } = new();

    protected IStateMachineListener<TState, TEvent> RelayStateListener
    {
        get
        {
            _relayListener ??= new StateMachineListenerRelay(StateListener);

            return _relayListener;
        }
    }

    public void Start()
    {
        IsRunning = true;
    }

    public void Stop()
    {
        IsRunning = false;
    }

    protected abstract void OnInit();


    protected void NotifyExtendedStateChanged(object key, object value, IStateContext<TState, TEvent> context)
    {
        StateListener.ExtendedStateChanged(key, value);
        StateListener.StateContext(context);
    }

    private class StateMachineListenerRelay(
        IStateMachineListener<TState, TEvent> delegateListener
    ) : IStateMachineListener<TState, TEvent>
    {
        public void StateChanged(IState<TState, TEvent> from, IState<TState, TEvent> to)
        {
            delegateListener.StateChanged(from, to);
        }

        public void StateEntered(IState<TState, TEvent> state)
        {
            delegateListener.StateEntered(state);
        }

        public void StateExited(IState<TState, TEvent> state)
        {
            delegateListener.StateExited(state);
        }

        public void EventNotAccepted(IMessage<TEvent> @event)
        {
            delegateListener.EventNotAccepted(@event);
        }

        public void Transition(ITransition<TState, TEvent> transition)
        {
            delegateListener.Transition(transition);
        }

        public void TransitionStarted(ITransition<TState, TEvent> transition)
        {
            delegateListener.TransitionStarted(transition);
        }

        public void TransitionEnded(ITransition<TState, TEvent> transition)
        {
            delegateListener.TransitionEnded(transition);
        }

        public void StateMachineStarted(IStateMachine<TState, TEvent> stateMachine)
        {
            delegateListener.StateMachineStarted(stateMachine);
        }

        public void StateMachineStopped(IStateMachine<TState, TEvent> stateMachine)
        {
            delegateListener.StateMachineStopped(stateMachine);
        }

        public void StateMachineError(IStateMachine<TState, TEvent> stateMachine, Exception? exception)
        {
            delegateListener.StateMachineError(stateMachine, exception);
        }

        public void ExtendedStateChanged(object key, object value)
        {
            delegateListener.ExtendedStateChanged(key, value);
        }

        public void StateContext(IStateContext<TState, TEvent> context)
        {
            delegateListener.StateContext(context);
        }
    }
}