using FlowStep.Core.Listener;

namespace FlowStep.Core.Trigger;

public class EventTrigger<TState, TEvent>(TEvent @event) : ITrigger<TState, TEvent>
{
    public bool Evaluate(ITriggerContext<TState, TEvent> context)
    {
        return context.Event != null && context.Event.Equals(Event);
    }

    public TEvent Event => @event;

    public void AddListener(ITriggerListener listener)
    {
        //No-op;
    }
}