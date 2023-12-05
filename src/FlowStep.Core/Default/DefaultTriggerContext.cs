using FlowStep.Core.Trigger;

namespace FlowStep.Core.Default;

internal class DefaultTriggerContext<TState, TEvent>(TEvent @event) : ITriggerContext<TState, TEvent>
{
    public TEvent Event => @event;
}