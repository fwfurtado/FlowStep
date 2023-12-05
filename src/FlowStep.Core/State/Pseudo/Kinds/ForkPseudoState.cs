using FlowStep.Core.Context;

namespace FlowStep.Core.State.Pseudo.Kinds;

public class ForkPseudoState<TState, TEvent>
(
    IEnumerable<IState<TState, TEvent>> forks
) : AbstractPseudoState<TState, TEvent>
{
    public override PseudoStateKind Kind => PseudoStateKind.Fork;
    public IEnumerable<IState<TState, TEvent>> Forks => forks;

    public override IState<TState, TEvent>? Entry(IStateContext<TState, TEvent> context)
    {
        return null;
    }

    public override void Exit(IStateContext<TState, TEvent> context)
    {
        //No-op
    }
}