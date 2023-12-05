using FlowStep.Core.Region;
using FlowStep.Core.Support;

namespace FlowStep.Core.Default;

internal record DefaultStateMachineEventResult<TState, TEvent>
(
    IRegion<TState, TEvent> Region,
    IMessage<TEvent> Message,
    ResultType Result,
    System.Action? OnComplete = null
) : IStateMachineEventResult<TState, TEvent>
{
    public void Complete() => OnComplete?.Invoke();
}