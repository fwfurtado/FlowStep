using System.Collections;

namespace FlowStep.Core.Support;

public class OrderedCompositeItem<T>
{
    private int _order = 0;
    private readonly SortedList<int, T> _items = new();


    public IEnumerable<T> Items
    {
        set
        {
            _items.Clear();
            _order = 0;

            foreach (var item in value)
            {
                Add(item);
            }
        }
    }

    public IEnumerable<T> Reverse => _items.Values.Reverse();


    public void Add(T item)
    {
        _items.Add(++_order, item);
    }


    public void Remove(T item)
    {
        var index = _items.IndexOfValue(item);

        if (index >= 0)
        {
            _items.RemoveAt(index);
        }
    }
}