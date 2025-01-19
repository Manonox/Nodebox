namespace Nodebox;


public static class IListExtensions {
    public static void Resize<T>(this IList<T> list, int size) {
		Resize(list, size, (_) => default);
    }

    public static void Resize<T>(this IList<T> list, int size, Func<int, T> generator) {
        ArgumentNullException.ThrowIfNull(list);
		ArgumentOutOfRangeException.ThrowIfNegative(size);

        if (list.Count == size) return;

        if (size > list.Count) {
            if (list is List<T> genericList) {
                genericList.AddRange(Enumerable.Range(0, size - list.Count).Select(generator));
            }
            else {
                while (list.Count < size)
                    list.Add(generator(list.Count));
            }
        } else {
            if (list is List<T> genericList) {
                genericList.RemoveRange(size, list.Count - size);
            }
            else {
                while (list.Count > size)
                    list.RemoveAt(list.Count - 1);
            }
        }
    }
}
