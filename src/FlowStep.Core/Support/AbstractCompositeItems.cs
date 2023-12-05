namespace FlowStep.Core.Support;

public class AbstractCompositeItems<T>
{
    private readonly OrderedCompositeItem<T> _orderedCompositeItems = new();


    public IEnumerable<T> Items
    {
        set => _orderedCompositeItems.Items = value;
        get => _orderedCompositeItems.Reverse;
    }


    public void Register(T item)
    {
        _orderedCompositeItems.Add(item);
    }

    public void Unregister(T item)
    {
        _orderedCompositeItems.Remove(item);
    }
}