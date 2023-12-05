namespace FlowStep.Core.Trigger;

public interface ITriggerContext<TState, out TEvent>
{
    TEvent Event { get; }
}