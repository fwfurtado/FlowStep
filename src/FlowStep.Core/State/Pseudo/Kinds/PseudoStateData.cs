using FlowStep.Core.Context;
using FlowStep.Core.Support;

namespace FlowStep.Core.State.Pseudo.Kinds;

public sealed class PseudoStateData<TState, TEvent>(
    StateHolder<TState, TEvent> holder,
    Guard<TState, TEvent> guard,
    IEnumerable<Action<IStateContext<TState, TEvent>>> actions
)
{
    public StateHolder<TState, TEvent> Holder => holder;
    public IState<TState, TEvent> State => holder.State;
    public Guard<TState, TEvent> Guard => guard;
    public IEnumerable<Action<IStateContext<TState, TEvent>>> Actions => actions;
}