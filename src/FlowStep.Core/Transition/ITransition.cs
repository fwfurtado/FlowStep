using FlowStep.Core.Action;
using FlowStep.Core.Context;
using FlowStep.Core.State;
using FlowStep.Core.Support;
using FlowStep.Core.Trigger;

namespace FlowStep.Core.Transition;

public interface ITransition<TState, TEvent>
{
    string Name { get; }
    bool CanTransit(IStateContext<TState, TEvent> context);
    void Execute(IStateContext<TState, TEvent> context);
    IState<TState, TEvent> Source { get; }
    IState<TState, TEvent> Target { get; }
    Guard<TState, TEvent> Guard { get; }
    IEnumerable<StateContextAction<TState, TEvent>> Actions { get; }
    ITrigger<TState, TEvent>? Trigger { get; }
    TransitionKind Kind { get; }
    void AddActionListener(IActionListener<TState, TEvent> listener);
    void RemoveActionListener(IActionListener<TState, TEvent> listener);
}