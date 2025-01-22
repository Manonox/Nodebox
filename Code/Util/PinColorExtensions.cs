using Sandbox.Internal;
namespace Nodebox;


public static class PinColorExtensions {
    public static Dictionary<Type, Color> ByType = new(new Dictionary<Type, Color>() {
        { typeof(object), new Color(1.0f, 0.25f, 1.0f) },
        { typeof(Polymorphic), Color.Gray.Darken(0.5f) },
        
        { typeof(bool), new Color(0.9f, 0.4f, 0.4f) },

        { typeof(float), new Color(0.25f, 0.75f, 1.0f) },
        { typeof(double), new Color(0.15f, 0.3f, 0.9f) },

        { typeof(byte), new Color(0.3f, 0.9f, 0.6f) },
        { typeof(int), new Color(0.3f, 0.9f, 0.6f) },
        { typeof(long), new Color(0.3f, 0.9f, 0.6f) },
        
        { typeof(char), new Color(0.2f, 0.15f, 0.6f) },
        { typeof(string), new Color(0.4f, 0.2f, 0.9f) },

        { typeof(Angles), new Color(0.4f, 0.8f, 0.3f) },
        { typeof(Rotation), new Color(0.5f, 0.8f, 0.5f) },

        { typeof(Color), new Color(1.0f, 0.6f, 0.3f) },

        { typeof(Reference), new Color(0.5f, 0.5f, 0.2f) },
        { typeof(GameObject), new Color(0.9f, 0.9f, 0.2f) },
    });

    private static readonly Dictionary<Type, Type> TypeAliases = new(new Dictionary<Type, Type>() {
        { typeof(Vector2), typeof(float) },
        { typeof(Vector3), typeof(float) },
        { typeof(Vector4), typeof(float) },

        { typeof(Vector2Int), typeof(int) },
        { typeof(Vector3Int), typeof(int) },
        //{ typeof(Vector4Int), typeof(int) },
    });

    
    public static Color GetColor(this Pin pin) => pin.Type.GetColor();
    public static Color GetColor(this Type type) {
        if (TypeAliases.TryGetValue(type, out var newType)) type = newType;
        if (ByType.TryGetValue(type, out var color)) return color;
    
        //typeLibrary.GetGenericTypes(type)
        return Color.Gray;
    }
}
