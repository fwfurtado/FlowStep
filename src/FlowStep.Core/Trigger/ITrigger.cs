using FlowStep.Core.Listener;

namespace FlowStep.Core.Trigger;

public interface ITrigger<TState, TEvent>
{
    bool Evaluate(ITriggerContext<TState, TEvent> context);

    TEvent Event { get; }

    void AddListener(ITriggerListener listener);
}