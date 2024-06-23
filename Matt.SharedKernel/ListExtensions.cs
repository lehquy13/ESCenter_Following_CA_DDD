namespace Matt.SharedKernel;

public static class ListExtensions
{
    public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> consumer)
    {
        foreach (var obj in enumerable)
            consumer(obj);
    }
}