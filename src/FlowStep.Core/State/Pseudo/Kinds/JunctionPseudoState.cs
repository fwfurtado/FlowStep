using FlowStep.Core.Context;
using FlowStep.Core.Listener;

namespace FlowStep.Core.State.Pseudo.Kinds;

public class JunctionPseudoState<TState, TEvent>
(
    IEnumerable<PseudoStateData<TState, TEvent>> junctions
) : IPseudoState<TState, TEvent>
{
    public PseudoStateKind Kind => PseudoStateKind.Junction;

    public IState<TState, TEvent>? Entry(IStateContext<TState, TEvent> context)
    {
        var evaluate = ContextEvaluatorBuilder(context);

        var junction = junctions.FirstOrDefault(evaluate);

        if (junction is null) return null;


        foreach (var action in junction.Actions)
        {
            action(context);
        }

        return junction.State;
    }

    private static Func<PseudoStateData<TState, TEvent>, bool> ContextEvaluatorBuilder(
        IStateContext<TState, TEvent> context
    )
    {
        return junction =>
        {
            try
            {
                return junction.Guard(context);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        };
    }

    public void Exit(IStateContext<TState, TEvent> context)
    {
        // No-op
    }

    public void AddPseudoStateListener(IPseudoStateListener<TState, TEvent> listener)
    {
        throw new NotImplementedException();
    }

    public void RemovePseudoStateListener(IPseudoStateListener<TState, TEvent> listener)
    {
        throw new NotImplementedException();
    }
}