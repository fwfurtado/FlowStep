using FlowStep.Core.Region;
using FlowStep.Core.State.Pseudo;
using FlowStep.Core.Support;

namespace FlowStep.Core.State;

public abstract class AbstractSimpleState<TState, TEvent>
(
    TState id,
    IPseudoState<TState, TEvent>? pseudoState = null,
    IEnumerable<StateContextAction<TState, TEvent>>? entryActions = null,
    IEnumerable<StateContextAction<TState, TEvent>>? stateActions = null,
    IEnumerable<StateContextAction<TState, TEvent>>? exitActions = null,
    IEnumerable<IRegion<TState, TEvent>>? regions = null,
    IStateMachine<TState, TEvent>? subStateMachine = null
) : AbstractState<TState, TEvent>(id, pseudoState, entryActions, exitActions, stateActions, regions, subStateMachine)
{
    private Lazy<IEnumerable<IState<TState, TEvent>>>? _states;
    private readonly List<TState> _ids = new(new[] { id });
    public override IEnumerable<TState> Ids => _ids;

    public override IEnumerable<IState<TState, TEvent>> States
    {
        get
        {
            if (_states is not null) return _states.Value;


            var states = new List<IState<TState, TEvent>> { this };
            _states = new Lazy<IEnumerable<IState<TState, TEvent>>>(states);

            return _states.Value;
        }
    }
}