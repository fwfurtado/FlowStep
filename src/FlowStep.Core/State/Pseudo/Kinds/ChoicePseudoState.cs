using FlowStep.Core.Context;
using FlowStep.Core.Listener;
using FlowStep.Core.Support;

namespace FlowStep.Core.State.Pseudo.Kinds;

public class ChoicePseudoState<TState, TEvent>
(
    List<ChoiceStateData<TState, TEvent>> choices
) : IPseudoState<TState, TEvent>
{
    public PseudoStateKind Kind => PseudoStateKind.Choice;

    public IState<TState, TEvent>? Entry(IStateContext<TState, TEvent> context)
    {
        var choice = choices.Find(c => c.Guard(context));

        if (choice is null)
        {
            return null;
        }

        foreach (var action in choice.Actions)
        {
            action(context);
        }

        return choice.State;
    }

    public void Exit(IStateContext<TState, TEvent> context)
    {
        //No-op
    }

    public void AddPseudoStateListener(IPseudoStateListener<TState, TEvent> listener)
    {
        //No-op
    }

    public void RemovePseudoStateListener(IPseudoStateListener<TState, TEvent> listener)
    {
        //No-op
    }
}

public class ChoiceStateData<TState, TEvent>
(
    StateHolder<TState, TEvent> holder,
    Guard<TState, TEvent> guard,
    IEnumerable<Action<IStateContext<TState, TEvent>>> actions
)
{
    public IState<TState, TEvent> State => holder.State;
    public Guard<TState, TEvent> Guard => guard;
    public IEnumerable<Action<IStateContext<TState, TEvent>>> Actions => actions;
}