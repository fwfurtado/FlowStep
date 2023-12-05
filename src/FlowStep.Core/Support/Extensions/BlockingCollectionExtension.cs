using System.Collections.Concurrent;

namespace FlowStep.Core.Support.Extensions;

internal static class BlockingCollectionExtension
{
    public static bool Remove<T>(this BlockingCollection<T> collection, T item)
    {
        var found = false;

        lock (collection)
        {
            var readItems = new List<T>();

            foreach (var readItem in collection.GetConsumingEnumerable())
            {
                if (readItem is not null && readItem.Equals(item))
                {
                    found = true;
                    continue;
                }

                readItems.Add(readItem);
            }

            Parallel.ForEach(readItems, collection.Add);
        }

        return found;
    }

    public static void Clear<T>(this BlockingCollection<T> collection)
    {
        lock (collection)
        {
            collection.ClearWithoutLock();
        }
    }

    private static void ClearWithoutLock<T>(this BlockingCollection<T> collection)
    {
        foreach (var _ in collection.GetConsumingEnumerable())
        {
        }
    }

    public static void ResetTo<T>(this BlockingCollection<T> collection, IEnumerable<T> items)
    {
        lock (collection)
        {
            collection.ClearWithoutLock();

            foreach (var item in items)
            {
                collection.Add(item);
            }
        }
    }
}