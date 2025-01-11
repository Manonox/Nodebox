namespace Nodebox;

public static class Util {
    public static IEnumerable<(int, T)> Enumerate<T>(this IEnumerable<T> ie)
    {
        var max = ie.Count();
        return Enumerable.Range(0, max).Zip(ie);
    }
    
    public static void ForEach<T>(this IEnumerable<T> ie, Action<T> action)
    {
        foreach (T item in ie) action(item);
    }
}
