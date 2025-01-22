namespace Nodebox;

public static class PackingExtensions {
    public static object PackVectorAny(this IEnumerable<float> ie) {
        var count = ie.Count();
        if (count >= 4)
            return ie.PackVector4();
        if (count >= 3)
            return ie.PackVector3();
        if (count >= 2)
            return ie.PackVector2();
        if (count >= 1)
            return ie.First();
        throw new ArgumentException("IEnumerable is empty");
    }

    public static Vector2 PackVector2(this IEnumerable<float> ie) {
        var v = ie.ToArray();
        return new(
            v[0],
            v[1]
        );
    }

    public static Vector3 PackVector3(this IEnumerable<float> ie) {
        var v = ie.ToArray();
        return new(
            v[0],
            v[1],
            v[2]
        );
    }

    public static Vector4 PackVector4(this IEnumerable<float> ie) {
        var v = ie.ToArray();
        return new(
            v[0],
            v[1],
            v[2],
            v[3]
        );
    }

    public static object PackVectorAnyInt(this IEnumerable<int> ie) {
        var count = ie.Count();
        if (count >= 3)
            return ie.PackVector3Int();
        if (count >= 2)
            return ie.PackVector2Int();
        if (count >= 1)
            return ie.First();
        throw new ArgumentException("IEnumerable is empty");
    }

    public static Vector2Int PackVector2Int(this IEnumerable<int> ie) {
        var v = ie.ToArray();
        return new(
            v[0],
            v[1]
        );
    }

    public static Vector3Int PackVector3Int(this IEnumerable<int> ie) {
        var v = ie.ToArray();
        return new(
            v[0],
            v[1],
            v[2]
        );
    }

    public static Angles PackAngles(this IEnumerable<float> ie) {
        var v = ie.ToArray();
        return new(
            v[0],
            v[1],
            v[2]
        );
    }

    public static Rotation PackRotation(this IEnumerable<float> ie) {
        var v = ie.ToArray();
        return new(
            v[0],
            v[1],
            v[2],
            v[3]
        );
    }

    public static Color PackColor(this IEnumerable<float> ie) {
        var v = ie.ToArray();
        return new(
            v[0],
            v[1],
            v[2],
            v[3]
        );
    }
    
    public static IEnumerable<float> Unpack(this Vector2 v) {
        yield return v.x;
        yield return v.y;
    }

    public static IEnumerable<float> Unpack(this Vector3 v) {
        yield return v.x;
        yield return v.y;
        yield return v.z;
    }

    public static IEnumerable<float> Unpack(this Vector4 v) {
        yield return v.x;
        yield return v.y;
        yield return v.z;
        yield return v.w;
    }

    public static IEnumerable<int> Unpack(this Vector2Int v) {
        yield return v.x;
        yield return v.y;
    }

    public static IEnumerable<int> Unpack(this Vector3Int v) {
        yield return v.x;
        yield return v.y;
        yield return v.z;
    }

    public static IEnumerable<float> Unpack(this Angles v) {
        yield return v.pitch;
        yield return v.yaw;
        yield return v.roll;
    }

    public static IEnumerable<float> Unpack(this Rotation v) {
        yield return v.x;
        yield return v.y;
        yield return v.z;
        yield return v.w;
    }

    public static IEnumerable<float> Unpack(this Color v) {
        yield return v.r;
        yield return v.g;
        yield return v.b;
        yield return v.a;
    }

    public static IEnumerable<float> UnpackAnyFloat(object v) => UnpackAnyFloat(v.GetType(), v);
    public static IEnumerable<float> UnpackAnyFloat<T>(T v) => UnpackAnyFloat(typeof(T), v);
    private static IEnumerable<float> UnpackAnyFloat(Type type, object v) {
        if (type == typeof(Vector2))
            return ((Vector2)v).Unpack();
        if (type == typeof(Vector3))
            return ((Vector3)v).Unpack();
        if (type == typeof(Vector4))
            return ((Vector4)v).Unpack();
        if (type == typeof(Angles))
            return ((Angles)v).Unpack();
        if (type == typeof(Rotation))
            return ((Rotation)v).Unpack();
        if (type == typeof(Color))
            return ((Color)v).Unpack();
        throw new NotImplementedException();
    }
    
    public static IEnumerable<int> UnpackAnyInt(object v) => UnpackAnyInt(v.GetType(), v);
    public static IEnumerable<int> UnpackAnyInt<T>(T v) => UnpackAnyInt(typeof(T), v);
    private static IEnumerable<int> UnpackAnyInt(Type type, object v) {
        if (type == typeof(Vector2Int))
            return ((Vector2Int)v).Unpack();
        if (type == typeof(Vector3Int))
            return ((Vector3Int)v).Unpack();
        throw new NotImplementedException();
    }
}
