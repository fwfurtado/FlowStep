using FlowStep.Core.Listener;

namespace FlowStep.Core.State.Extended;

public interface IExtendedState
{
    IDictionary<object, object> Variables { get; }
    IExtendedStateListener Listener { get; }
    T? Get<T>(object key);
}