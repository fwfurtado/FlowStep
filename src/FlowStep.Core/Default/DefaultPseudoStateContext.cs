using FlowStep.Core.State.Pseudo;

namespace FlowStep.Core.Default;

internal class DefaultPseudoStateContext<TState, TEvent>
(
    IPseudoState<TState, TEvent> pseudoState,
    PseudoStateAction action = PseudoStateAction.JoinComplete
) : IPseudoStateContext<TState, TEvent>
{
    public IPseudoState<TState, TEvent> PseudoState => pseudoState;
    public PseudoStateAction Action => action;
}