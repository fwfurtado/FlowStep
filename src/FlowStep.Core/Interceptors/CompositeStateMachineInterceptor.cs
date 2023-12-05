using System.Collections;
using System.Collections.Concurrent;
using FlowStep.Core.Context;
using FlowStep.Core.State;
using FlowStep.Core.Support;
using FlowStep.Core.Support.Extensions;
using FlowStep.Core.Transition;

namespace FlowStep.Core.Interceptors;

public class CompositeStateMachineInterceptor<TState, TEvent> :
    IStateMachineInterceptor<TState, TEvent>,
    IEnumerable<IStateMachineInterceptor<TState, TEvent>>
{
    private readonly BlockingCollection<IStateMachineInterceptor<TState, TEvent>> _interceptors = new();

    public void ResetTo(IEnumerable<IStateMachineInterceptor<TState, TEvent>> interceptors)
    {
        _interceptors.ResetTo(interceptors);
    }


    public void Add(IStateMachineInterceptor<TState, TEvent> interceptor)
    {
        _interceptors.Add(interceptor);
    }

    public void Remove(IStateMachineInterceptor<TState, TEvent> interceptor)
    {
        _interceptors.Remove(interceptor);
    }

    public IMessage<TEvent> PreEvent(IMessage<TEvent> message, IStateMachine<TState, TEvent> machine)
    {
        return _interceptors.Aggregate(message, (current, interceptor) => interceptor.PreEvent(current, machine));
    }

    public void PreStateChange(
        IState<TState, TEvent> state,
        IMessage<TEvent> message,
        ITransition<TState, TEvent> transition,
        IStateMachine<TState, TEvent> machine,
        IStateMachine<TState, TEvent> rootMachine
    )
    {
        foreach (var interceptor in _interceptors)
        {
            interceptor.PreStateChange(state, message, transition, machine, rootMachine);
        }
    }

    public void PostStateChange(
        IState<TState, TEvent> state,
        IMessage<TEvent> message,
        ITransition<TState, TEvent> transition,
        IStateMachine<TState, TEvent> machine,
        IStateMachine<TState, TEvent> rootMachine
    )
    {
        foreach (var interceptor in _interceptors)
        {
            interceptor.PostStateChange(state, message, transition, machine, rootMachine);
        }
    }

    public IStateContext<TState, TEvent> PreTransition(
        IStateContext<TState, TEvent> context
    )
    {
        return _interceptors.Aggregate(context, (current, interceptor) => interceptor.PreTransition(current));
    }

    public IStateContext<TState, TEvent> PostTransition(
        IStateContext<TState, TEvent> context
    )
    {
        return _interceptors.Aggregate(context, (current, interceptor) => interceptor.PostTransition(current));
    }

    public Exception Error(
        IStateMachine<TState, TEvent> machine, Exception exception
    )
    {
        return _interceptors.Aggregate(exception, (current, interceptor) => interceptor.Error(machine, current));
    }

    public IEnumerator<IStateMachineInterceptor<TState, TEvent>> GetEnumerator()
    {
        foreach (var interceptor in _interceptors)
        {
            yield return interceptor;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}