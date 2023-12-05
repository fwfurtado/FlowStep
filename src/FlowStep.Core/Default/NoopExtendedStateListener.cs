using FlowStep.Core.Listener;

namespace FlowStep.Core.Default;

public class NoopExtendedStateListener : IExtendedStateListener
{
    public void Changed(object key, object value)
    {
        // No-op
    }
}