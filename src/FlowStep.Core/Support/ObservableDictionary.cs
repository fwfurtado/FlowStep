using System.Collections;
using System.Collections.Concurrent;

namespace FlowStep.Core.Support;

public class ObservableDictionary<TKey, TValue>
(
    Dictionary<TKey, TValue>? delegateDictionary = null,
    IDictionaryChangeListener<TKey, TValue>? listener = null
) : IDictionary<TKey, TValue> where TKey : notnull
{
    private readonly ConcurrentDictionary<TKey, TValue> _delegate =
        new(delegateDictionary ?? new Dictionary<TKey, TValue>());


    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        return _delegate.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_delegate).GetEnumerator();
    }

    public void Add(KeyValuePair<TKey, TValue> item)
    {
        var (key, value) = item;
        Add(key, value);
    }

    public void Add(TKey key, TValue value)
    {
        _delegate.AddOrUpdate(key,
            _ =>
            {
                listener?.Added(key, value);
                return value;
            },
            (_, _) =>
            {
                listener?.Updated(key, value);
                return value;
            });
    }

    public void Clear()
    {
        _delegate.Clear();
    }

    public bool Contains(KeyValuePair<TKey, TValue> item)
    {
        return _delegate.Contains(item);
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        foreach (var (key, value) in array.Skip(arrayIndex))
        {
            Add(key, value);
        }
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        return Remove(item.Key);
    }

    public bool Remove(TKey key)
    {
        var removed = _delegate.Remove(key, out var value);

        if (value is not null && removed)
            listener?.Removed(key, value);

        return removed;
    }

    public int Count => _delegate.Count;

    public bool IsReadOnly => false;


    public bool ContainsKey(TKey key)
    {
        return _delegate.ContainsKey(key);
    }


    public bool TryGetValue(TKey key, out TValue value)
    {
        var found = _delegate.TryGetValue(key, out var myValue);

        if (myValue is not null)
        {
            value = myValue;

            return true;
        }


        value = default!;

        return found;
    }

    public TValue this[TKey key]
    {
        get => _delegate[key];
        set => Add(key, value);
    }

    public ICollection<TKey> Keys => _delegate.Keys;

    public ICollection<TValue> Values => _delegate.Values;
}