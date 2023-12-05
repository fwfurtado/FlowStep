namespace FlowStep.Core.Support;

public interface IDictionaryChangeListener<in TKey, in TValue>
{
    void Added(TKey key, TValue value);
    void Updated(TKey key, TValue value);
    void Removed(TKey key, TValue value);
}