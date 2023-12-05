using FlowStep.Core.Context;
using FlowStep.Core.Listener;
using FlowStep.Core.State;
using FlowStep.Core.Support;
using FlowStep.Core.Trigger;

namespace FlowStep.Core.Transition;

public class InitialTransition<TState, TEvent>
(
    IState<TState, TEvent> target,
    IState<TState, TEvent>? source = null,
    IEnumerable<StateContextAction<TState, TEvent>>? actions = null,
    Guard<TState, TEvent>? guard = null,
    ITrigger<TState, TEvent>? trigger = null,
    string name = "",
    CompositeActionListener<TState, TEvent>? compositeActionListener = null
) : AbstractTransition<TState, TEvent>
(
    source,
    target,
    actions,
    TransitionKind.Initial,
    guard,
    trigger,
    name,
    compositeActionListener
)
{
    
    public override bool CanTransit(IStateContext<TState, TEvent> context)
    {
        return false;
    }
}