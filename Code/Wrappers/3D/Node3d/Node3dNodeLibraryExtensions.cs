namespace Nodebox;

public static class Node3dNodeLibraryExtensions {
    public static GameObject CreateNode3d(this Library.Entry entry) => Node3d.Wrap(entry.CreateNode());
}
