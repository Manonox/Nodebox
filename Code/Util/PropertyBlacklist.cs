namespace Nodebox;


public static class Blacklist {
    public static HashSet<Type> Types = new() {};
    public static Dictionary<Type, HashSet<string>> Properties = new() {
        { typeof(GameObject), new() {
            nameof(GameObject.IsProxy),
            nameof(GameObject.IsDestroyed), // ?
            nameof(GameObject.Active),
            nameof(GameObject.IsPrefabInstance),
            nameof(GameObject.IsPrefabInstanceRoot),
            nameof(GameObject.HasGizmoHandle),
            } },

        { typeof(Component), new() {
            nameof(Component.WorldTransform),
            nameof(Component.WorldPosition),
            nameof(Component.WorldRotation),
            nameof(Component.WorldScale),

            nameof(Component.LocalTransform),
            nameof(Component.LocalPosition),
            nameof(Component.LocalRotation),
            nameof(Component.LocalScale),

            nameof(Component.IsProxy),
            nameof(Component.Active),
            nameof(Component.ComponentVersion),
            nameof(Component.GameObject),
            } },
    };
}
