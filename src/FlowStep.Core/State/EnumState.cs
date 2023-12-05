using FlowStep.Core.Region;
using FlowStep.Core.State.Pseudo;
using FlowStep.Core.Support;

namespace FlowStep.Core.State;

public class EnumState<TState, TEvent>
(
    TState id,
    IPseudoState<TState, TEvent>? pseudoState = null,
    IEnumerable<StateContextAction<TState, TEvent>>? entryActions = null,
    IEnumerable<StateContextAction<TState, TEvent>>? stateActions = null,
    IEnumerable<StateContextAction<TState, TEvent>>? exitActions = null,
    IEnumerable<IRegion<TState, TEvent>>? regions = null,
    IStateMachine<TState, TEvent>? subStateMachine = null
) : ObjectState<TState, TEvent>(
    id,
    pseudoState,
    entryActions,
    exitActions,
    stateActions,
    regions,
    subStateMachine
) where TState : Enum where TEvent : Enum;