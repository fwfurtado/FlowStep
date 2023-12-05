using FlowStep.Core.Listener;
using FlowStep.Core.State;
using FlowStep.Core.Support;
using FlowStep.Core.Transition;

namespace FlowStep.Core.Region;

public interface IRegion<TState, TEvent> : IStateMachineLifecycle
{
    Guid Uuid { get; }
    string Id { get; }
    bool IsComplete { get; }
    IState<TState, TEvent> State { get; }
    IEnumerable<IState<TState, TEvent>> States { get; }
    IEnumerable<ITransition<TState, TEvent>> Transitions { get; }


    IEnumerable<IStateMachineEventResult<TState, TEvent>> SendEvents(IEnumerable<IMessage<TEvent>> events);
    IEnumerable<IStateMachineEventResult<TState, TEvent>> SendEvent(IMessage<TEvent> @event);

    void AddStateListener(IStateMachineListener<TState, TEvent> listener);
    void RemoveStateListener(IStateMachineListener<TState, TEvent> listener);
}