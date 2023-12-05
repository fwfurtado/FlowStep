using FlowStep.Core.Context;
using FlowStep.Core.Region;
using FlowStep.Core.State.Pseudo;
using FlowStep.Core.Support;

namespace FlowStep.Core.State;

public class ObjectState<TState, TEvent>(
    TState id,
    IPseudoState<TState, TEvent>? pseudoState = null,
    IEnumerable<StateContextAction<TState, TEvent>>? entryActions = null,
    IEnumerable<StateContextAction<TState, TEvent>>? stateActions = null,
    IEnumerable<StateContextAction<TState, TEvent>>? exitActions = null,
    IEnumerable<IRegion<TState, TEvent>>? regions = null,
    IStateMachine<TState, TEvent>? subStateMachine = null
) : AbstractSimpleState<TState, TEvent>(
    id,
    pseudoState,
    entryActions,
    stateActions,
    exitActions,
    regions,
    subStateMachine
)
{
    public override void Entry(IStateContext<TState, TEvent> context)
    {
        if (entryActions is null) return;

        foreach (var action in entryActions)
        {
            ExecuteAction(action, context);
        }

        base.Entry(context);
    }

    public override void Exit(IStateContext<TState, TEvent> context)
    {
        base.Exit(context);

        if (exitActions is null) return;

        foreach (var action in exitActions)
        {
            ExecuteAction(action, context);
        }
    }
}