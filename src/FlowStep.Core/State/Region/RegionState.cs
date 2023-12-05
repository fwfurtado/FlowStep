using System.Collections.Concurrent;
using FlowStep.Core.Context;
using FlowStep.Core.Region;
using FlowStep.Core.State.Pseudo;
using FlowStep.Core.Support;
using FlowStep.Core.Support.Extensions;

namespace FlowStep.Core.State.Region;

public class RegionState<TState, TEvent>
(
    TState id,
    RegionExecutionPolicy policy,
    IEnumerable<IRegion<TState, TEvent>> regions,
    IEnumerable<StateContextAction<TState, TEvent>>? entryActions = null,
    IEnumerable<StateContextAction<TState, TEvent>>? stateActions = null,
    IEnumerable<StateContextAction<TState, TEvent>>? exitActions = null,
    IPseudoState<TState, TEvent>? pseudo = null
) : AbstractState<TState, TEvent>(
    id,
    pseudo,
    entryActions,
    stateActions,
    exitActions,
    regions
)
{
    public RegionExecutionPolicy Policy { get; } = policy;

    public override IEnumerable<TState> Ids
    {
        get
        {
            yield return Id;
            foreach (var region in regions)
            {
                var state = region.State;

                foreach (var stateId in state.Ids)
                {
                    yield return stateId;
                }
            }
        }
    }

    public override IEnumerable<IState<TState, TEvent>> States
    {
        get
        {
            yield return this;

            foreach (var region in regions)
            {
                foreach (var state in region.States)
                {
                    yield return state;
                }
            }
        }
    }

    public override IEnumerable<IStateMachineEventResult<TState, TEvent>> Send(IMessage<TEvent> @event)
    {
        var results = new ConcurrentBag<IStateMachineEventResult<TState, TEvent>>();

        regions.Do(policy,
            region => results.AddRange(region.State.Send(@event))
        );

        return results;
    }


    public override void Entry(IStateContext<TState, TEvent> context)
    {
        base.Entry(context);

        if (entryActions is not null)
        {
            foreach (var action in entryActions)
            {
                ExecuteAction(action, context);
            }
        }

        StartOrEntry(context);
    }


    private void StartOrEntry(IStateContext<TState, TEvent> context)
    {
        if (pseudo is { Kind: PseudoStateKind.Initial })
        {
            regions
                .Where(r => r.States.None(context.IsInTargets))
                .Do(policy, region => region.Start());
        }


        foreach (var region in regions)
        {
            region.State.Entry(context);
        }
    }

    public override void Exit(IStateContext<TState, TEvent> context)
    {
        base.Exit(context);

        if (exitActions is not null)
        {
            foreach (var action in exitActions)
            {
                ExecuteAction(action, context);
            }
        }

        foreach (var region in regions)
        {
            region.Stop();
        }
    }
}