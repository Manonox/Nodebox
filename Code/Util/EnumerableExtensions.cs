namespace Nodebox;

public static class EnumerableExtensions {
    public static IEnumerable<(int Index, T Item)> Enumerate<T>(this IEnumerable<T> ie)
    {
        var max = ie.Count();
        return Enumerable.Range(0, max).Zip(ie);
    }
    
    public static void ForEach<T>(this IEnumerable<T> ie, Action<T> action)
    {
        foreach (T item in ie) action(item);
    }
    
    public static IEnumerable<TResult> SelectWhere<TSource, TResult>(this IEnumerable<TSource> ie, Func<TSource, TResult> action)
    {
        return ie.SelectMany(x => {
            var result = action(x);
            return result == null ? Enumerable.Empty<TResult>() : [result];
        });
    }

    public static IEnumerable<(T, T)> Pairs<T>(this IEnumerable<T> ie)
    {
        if (!ie.Any()) yield break;
        var previous = ie.First();
        ie = ie.Skip(1);
        while (ie.Any()) {
            var current = ie.First();
            yield return (previous, current);
            previous = current;
            ie = ie.Skip(1);
        }
    }
}
