using FlowStep.Core.Default;
using FlowStep.Core.Region;
using FlowStep.Core.Support;

namespace FlowStep.Core;

public interface IStateMachineEventResult<TState, TEvent>
{
    IRegion<TState, TEvent> Region { get; }
    IMessage<TEvent> Message { get; }
    ResultType Result { get; }

    void Complete();

    public static IStateMachineEventResult<TState, TEvent> From(
        IRegion<TState, TEvent> region,
        IMessage<TEvent> message,
        ResultType result,
        System.Action? onComplete = null
    ) =>
        new DefaultStateMachineEventResult<TState, TEvent>(
            region,
            message,
            result,
            onComplete
        );
}

public enum ResultType
{
    Accepted,
    Denied,
    Deferred
}