using FlowStep.Core.Action;
using FlowStep.Core.Context;
using FlowStep.Core.Listener;
using FlowStep.Core.State;
using FlowStep.Core.Support;
using FlowStep.Core.Trigger;

namespace FlowStep.Core.Transition;

public class AbstractTransition<TState, TEvent>
(
    IState<TState, TEvent> source,
    IState<TState, TEvent> target,
    IEnumerable<StateContextAction<TState, TEvent>> actions,
    TransitionKind kind,
    Guard<TState, TEvent> guard,
    ITrigger<TState, TEvent> trigger,
    string name = "",
    CompositeActionListener<TState, TEvent>? compositeActionListener = null
) : ITransition<TState, TEvent>
{
    private readonly CompositeActionListener<TState, TEvent> _compositeActionListener =
        compositeActionListener ?? new CompositeActionListener<TState, TEvent>();

    public string Name { get; } = name;

    public virtual bool CanTransit(IStateContext<TState, TEvent> context)
    {
        return guard(context);
    }

    public void Execute(IStateContext<TState, TEvent> context)
    {
        foreach (var action in actions)
        {
            var now = DateTime.UtcNow;

            action(context);

            var duration = DateTime.UtcNow - now;

            _compositeActionListener.OnExecute(context.StateMachine, action, duration.Milliseconds);
        }
    }

    public IState<TState, TEvent> Source { get; } = source;
    public IState<TState, TEvent> Target { get; } = target;
    public Guard<TState, TEvent> Guard { get; } = guard;
    public IEnumerable<StateContextAction<TState, TEvent>> Actions { get; } = actions;
    public ITrigger<TState, TEvent> Trigger { get; } = trigger;
    public TransitionKind Kind { get; } = kind;

    public void AddActionListener(IActionListener<TState, TEvent> listener)
    {
        _compositeActionListener.Register(listener);
    }

    public void RemoveActionListener(IActionListener<TState, TEvent> listener)
    {
        _compositeActionListener.Unregister(listener);
    }
}

public class AbstractLocalTransition<TState, TEvent>
(
    IState<TState, TEvent> source,
    IState<TState, TEvent> target,
    IEnumerable<StateContextAction<TState, TEvent>> actions,
    Guard<TState, TEvent> guard,
    ITrigger<TState, TEvent> trigger,
    string name = "",
    CompositeActionListener<TState, TEvent>? compositeActionListener = null
) : AbstractTransition<TState, TEvent>
(
    source,
    target,
    actions,
    TransitionKind.Local,
    guard,
    trigger,
    name,
    compositeActionListener
);

public class AbstractInternalTransition<TState, TEvent>
(
    IState<TState, TEvent> source,
    IState<TState, TEvent> target,
    IEnumerable<StateContextAction<TState, TEvent>> actions,
    Guard<TState, TEvent> guard,
    ITrigger<TState, TEvent> trigger,
    string name = "",
    CompositeActionListener<TState, TEvent>? compositeActionListener = null
) : AbstractTransition<TState, TEvent>
(
    source,
    target,
    actions,
    TransitionKind.Internal,
    guard,
    trigger,
    name,
    compositeActionListener
);

public class AbstractExternalTransition<TState, TEvent>
(
    IState<TState, TEvent> source,
    IState<TState, TEvent> target,
    IEnumerable<StateContextAction<TState, TEvent>> actions,
    Guard<TState, TEvent> guard,
    ITrigger<TState, TEvent> trigger,
    string name = "",
    CompositeActionListener<TState, TEvent>? compositeActionListener = null
) : AbstractTransition<TState, TEvent>
(
    source,
    target,
    actions,
    TransitionKind.External,
    guard,
    trigger,
    name,
    compositeActionListener
);