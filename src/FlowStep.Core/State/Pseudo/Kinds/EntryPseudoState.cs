using FlowStep.Core.Context;
using FlowStep.Core.Listener;

namespace FlowStep.Core.State.Pseudo.Kinds;

public class EntryPseudoState<TState, TEvent>(IState<TState, TEvent> state) : IPseudoState<TState, TEvent>
{
    public PseudoStateKind Kind => PseudoStateKind.Entry;
    public IState<TState, TEvent> State => state;

    public IState<TState, TEvent>? Entry(IStateContext<TState, TEvent> context)
    {
        return state;
    }

    public void Exit(IStateContext<TState, TEvent> context)
    {
        // No-op
    }

    public void AddPseudoStateListener(IPseudoStateListener<TState, TEvent> listener)
    {
        // No-op
    }

    public void RemovePseudoStateListener(IPseudoStateListener<TState, TEvent> listener)
    {
        // No-op
    }
}