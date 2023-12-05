using FlowStep.Core.Context;
using FlowStep.Core.Listener;

namespace FlowStep.Core.State.Pseudo.Kinds;

public abstract class AbstractPseudoState<TState, TEvent> : IPseudoState<TState, TEvent>
{
    private readonly CompositePseudoStateListener<TState, TEvent> _listeners =
        new();

    public abstract PseudoStateKind Kind { get; }

    public abstract IState<TState, TEvent>? Entry(IStateContext<TState, TEvent> context);

    public abstract void Exit(IStateContext<TState, TEvent> context);

    public void AddPseudoStateListener(IPseudoStateListener<TState, TEvent> listener)
    {
        _listeners.Register(listener);
    }

    public void RemovePseudoStateListener(IPseudoStateListener<TState, TEvent> listener)
    {
        _listeners.Unregister(listener);
    }

    protected void Notify(IPseudoStateContext<TState, TEvent> context)
    {
        _listeners.OnContext(context);
    }
}