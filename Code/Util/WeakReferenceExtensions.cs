namespace Nodebox;

public static class WeakReferenceExtensions {
    public static T GetTarget<T>(this WeakReference<T> weakReference) where T : class {
        if (!weakReference.TryGetTarget(out var target)) {
            return null;
        }
        
        return target;
    }
}