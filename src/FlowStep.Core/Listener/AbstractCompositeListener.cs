using FlowStep.Core.Support;

namespace FlowStep.Core.Listener;

public abstract class AbstractCompositeListener<T>
{
    private readonly OrderedCompositeItem<T> _listeners = new();

    protected CompositeListenerEnumerableSemantic<T> Listeners => new(_listeners);

    public void Register(T listener)
    {
        _listeners.Add(listener);
    }

    public void Unregister(T listener)
    {
        _listeners.Remove(listener);
    }
}

public readonly struct CompositeListenerEnumerableSemantic<T>(OrderedCompositeItem<T> listeners)
{
    public IEnumerable<T> InReverseOrder => listeners.Reverse;
}