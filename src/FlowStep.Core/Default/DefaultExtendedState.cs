using FlowStep.Core.Listener;
using FlowStep.Core.State.Extended;
using FlowStep.Core.Support;

namespace FlowStep.Core.Default;

public class DefaultExtendedState
(
    Dictionary<object, object>? variables = null,
    IExtendedStateListener? listener = null
) : IExtendedState
{
    public IDictionary<object, object> Variables { get; } = new ObservableDictionary<object, object>();
    public IExtendedStateListener Listener { get; } = listener ?? new NoopExtendedStateListener();

    public T? Get<T>(object key)
    {
        if (!Variables.TryGetValue(key, out var value)) return default;

        if (value is T variable)
        {
            return variable;
        }

        return default;
    }
}