using FlowStep.Core.Context;
using FlowStep.Core.Listener;

namespace FlowStep.Core.State.Pseudo;

public interface IPseudoState<TState, TEvent>
{
    PseudoStateKind Kind { get; }

    IState<TState, TEvent>? Entry(IStateContext<TState, TEvent> context);
    void Exit(IStateContext<TState, TEvent> context);
    
    void AddPseudoStateListener(IPseudoStateListener<TState, TEvent> listener);
    void RemovePseudoStateListener(IPseudoStateListener<TState, TEvent> listener);
    
}