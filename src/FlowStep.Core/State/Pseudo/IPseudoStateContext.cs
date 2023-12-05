namespace FlowStep.Core.State.Pseudo;

public interface IPseudoStateContext<TState, TEvent>
{
    IPseudoState<TState, TEvent> PseudoState { get; }

    PseudoStateAction Action { get; }

}