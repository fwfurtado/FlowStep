using FlowStep.Core.State;
using FlowStep.Core.State.Extended;
using FlowStep.Core.Support;
using FlowStep.Core.Transition;

namespace FlowStep.Core.Context;

public interface IStateContext<TState, TEvent>
{
    Stage Stage { get; }
    IMessage<TEvent>? Message { get; }
    TEvent? Event { get; }
    MessageHeaders? Headers { get; }
    object? GetHeader(string key);

    IExtendedState? ExtendedState { get; }

    ITransition<TState, TEvent>? Transition { get; }

    IStateMachine<TState, TEvent> StateMachine { get; }
    IState<TState, TEvent>? Source { get; }
    IEnumerable<IState<TState, TEvent>>? Sources { get; }
    IState<TState, TEvent>? Target { get; }
    IEnumerable<IState<TState, TEvent>>? Targets { get; }

    Exception? Error { get; }


    public bool IsInTargets(IState<TState, TEvent> state)
    {
        return Targets?.Contains(state) ?? false;
    }
}

[Flags]
public enum Stage
{
    EventNotAccepted = 0,
    ExtendedStateChanged,
    StateChanged,
    StateEntry,
    StateExit,
    StateMachineError,
    StateMachineStart,
    StateMachineStop,
    Transition,
    TransitionStart,
    TransitionEnd
}