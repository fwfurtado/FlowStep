namespace FlowStep.Core.Listener;

public class CompositeTriggerListener : AbstractCompositeListener<ITriggerListener>, ITriggerListener
{
    public void Triggered()
    {
        foreach (var listener in Listeners.InReverseOrder)
        {
            listener.Triggered();
        }
    }
}