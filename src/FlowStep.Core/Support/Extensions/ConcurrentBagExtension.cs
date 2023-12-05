using System.Collections.Concurrent;

namespace FlowStep.Core.Support.Extensions;

internal static class ConcurrentBagExtension
{
    public static void AddRange<T>(this ConcurrentBag<T> bag, IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            bag.Add(item);
        }
    }
}