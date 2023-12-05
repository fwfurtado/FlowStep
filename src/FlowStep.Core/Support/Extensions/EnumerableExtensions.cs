using FlowStep.Core.Region;

namespace FlowStep.Core.Support.Extensions;

internal static class EnumerableExtensions
{
    public static void Do<T>(this IEnumerable<T> enumerable, RegionExecutionPolicy policy, Action<T> action)
    {
        switch (policy)
        {
            case RegionExecutionPolicy.Sequential:
                foreach (var item in enumerable)
                {
                    action(item);
                }

                break;
            case RegionExecutionPolicy.Parallel:
                Parallel.ForEach(enumerable, action);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(policy), policy, null);
        }
    }

    public static bool None<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
    {
        return !enumerable.Any(predicate);
    }


    public static bool ContainsAny<T>(this IEnumerable<T> enumerable, IEnumerable<T> other)
    {
        return enumerable.Any(other.Contains);
    }
    
    public static bool ContainsNone<T>(this IEnumerable<T> enumerable, IEnumerable<T> other)
    {
        return !enumerable.Any(other.Contains);
    }
}