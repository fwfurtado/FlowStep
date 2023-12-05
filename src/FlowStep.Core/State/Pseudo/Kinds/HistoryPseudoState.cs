using FlowStep.Core.Context;

namespace FlowStep.Core.State.Pseudo.Kinds;

public class HistoryPseudoState<TState, TEvent> : AbstractPseudoState<TState, TEvent>
{
    private const PseudoStateKind IsHistory = PseudoStateKind.HistoryShallow | PseudoStateKind.HistoryDeep;

    private readonly StateHolder<TState, TEvent> _defaultState;
    private readonly StateHolder<TState, TEvent>? _containingState;

    public override PseudoStateKind Kind { get; }
    public IState<TState, TEvent>? State { get; set; }

    public HistoryPseudoState(
        PseudoStateKind kind,
        StateHolder<TState, TEvent> defaultState,
        StateHolder<TState, TEvent>? containingState = null
    )
    {
        if (IsHistory.HasNotFlag(kind))
        {
            throw new ArgumentException($"Pseudo state kind {kind} is not a history pseudo state.");
        }

        Kind = kind;
        _defaultState = defaultState;
        _containingState = containingState;
    }


    public override IState<TState, TEvent>? Entry(IStateContext<TState, TEvent> context)
    {
        if (State is null)
        {
            return _defaultState.State;
        }

        return State.Pseudo is { Kind: PseudoStateKind.End } ? _defaultState.State : State;
    }

    public override void Exit(IStateContext<TState, TEvent> context)
    {
        //No-op
    }
}