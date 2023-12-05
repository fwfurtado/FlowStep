using FlowStep.Core.Region;
using FlowStep.Core.State;
using FlowStep.Core.State.Extended;

namespace FlowStep.Core;

public interface IStateMachine<TState, TEvent> : IRegion<TState, TEvent>
{
    IState<TState, TEvent> Initial { get; }

    IExtendedState? ExtendedState { get; }

    Exception? Error { get; }
    bool HasError { get; }
}