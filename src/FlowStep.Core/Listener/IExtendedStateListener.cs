using FlowStep.Core.Support;

namespace FlowStep.Core.Listener;

public interface IExtendedStateListener
{
    void Changed(object key, object value);
}

public class ExtendedStateListenerByAction
(
    ExtendedStateChanged extendedStateChanged
) : IExtendedStateListener
{
    public void Changed(object key, object value)
    {
        extendedStateChanged(key, value);
    }
}