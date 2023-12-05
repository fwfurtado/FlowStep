using FlowStep.Core.State.Extended;

namespace FlowStep.Core.Context;

public interface IStateMachineContext<TState, TEvent> where TState : notnull
{
    string Id { get; }

    IEnumerable<IStateMachineContext<TState, TEvent>> Children { get; }


    Dictionary<TState, TState> History { get; }
    Dictionary<string, object> EventHeaders { get; }

    TState State { get; }
    TEvent Event { get; }

    IExtendedState? ExtendedState { get; }
}