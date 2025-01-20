namespace Nodebox;

public static class VectorExtensions {
    public static Vector2 Modulo(this Vector2 v, Vector2 other) => new(
        v.x % other.x,
        v.y % other.y
    );

    public static Vector3 Modulo(this Vector3 v, Vector3 other) => new(
        v.x % other.x,
        v.y % other.y,
        v.z % other.z
    );

    public static Vector4 Modulo(this Vector4 v, Vector4 other) => new(
        v.x % other.x,
        v.y % other.y,
        v.z % other.z,
        v.w % other.w
    );
    
    public static Vector2Int Modulo(this Vector2Int v, Vector2Int other) => new(
        v.x % other.x,
        v.y % other.y
    );

    public static Vector3Int Modulo(this Vector3Int v, Vector3Int other) => new(
        v.x % other.x,
        v.y % other.y,
        v.z % other.z
    );


    public static Vector4 Abs(this Vector4 v) => new(
        MathF.Abs(v.x),
        MathF.Abs(v.y),
        MathF.Abs(v.z),
        MathF.Abs(v.w)
    );

    public static Color Abs(this Color v) => new(
        MathF.Abs(v.r),
        MathF.Abs(v.g),
        MathF.Abs(v.b),
        MathF.Abs(v.a)
    );


    public static Vector2 Floor(this Vector2 v) => new(
        MathF.Floor(v.x),
        MathF.Floor(v.y)
    );
    
    public static Vector3 Floor(this Vector3 v) => new(
        MathF.Floor(v.x),
        MathF.Floor(v.y),
        MathF.Floor(v.z)
    );
    
    public static Vector4 Floor(this Vector4 v) => new(
        MathF.Floor(v.x),
        MathF.Floor(v.y),
        MathF.Floor(v.z),
        MathF.Floor(v.w)
    );
    
    public static Vector2 Ceiling(this Vector2 v) => new(
        MathF.Ceiling(v.x),
        MathF.Ceiling(v.y)
    );
    
    public static Vector3 Ceiling(this Vector3 v) => new(
        MathF.Ceiling(v.x),
        MathF.Ceiling(v.y),
        MathF.Ceiling(v.z)
    );
    
    public static Vector4 Ceiling(this Vector4 v) => new(
        MathF.Ceiling(v.x),
        MathF.Ceiling(v.y),
        MathF.Ceiling(v.z),
        MathF.Ceiling(v.w)
    );

    public static Vector2 Truncate(this Vector2 v) => new(
        MathF.Truncate(v.x),
        MathF.Truncate(v.y)
    );
    
    public static Vector3 Truncate(this Vector3 v) => new(
        MathF.Truncate(v.x),
        MathF.Truncate(v.y),
        MathF.Truncate(v.z)
    );
    
    public static Vector4 Truncate(this Vector4 v) => new(
        MathF.Truncate(v.x),
        MathF.Truncate(v.y),
        MathF.Truncate(v.z),
        MathF.Truncate(v.w)
    );

    
    public static Vector4 Clamp(this Vector4 v, Vector4 min, Vector4 max) => new(
        Math.Clamp(v.x, min.x, max.x),
        Math.Clamp(v.y, min.y, max.y),
        Math.Clamp(v.z, min.z, max.z),
        Math.Clamp(v.w, min.w, max.w)
    );
    
    public static Vector2Int Clamp(this Vector2Int v, Vector2Int min, Vector2Int max) => new(
        Math.Clamp(v.x, min.x, max.x),
        Math.Clamp(v.y, min.y, max.y)
    );
    
    public static Vector3Int Clamp(this Vector3Int v, Vector3Int min, Vector3Int max) => new(
        Math.Clamp(v.x, min.x, max.x),
        Math.Clamp(v.y, min.y, max.y),
        Math.Clamp(v.z, min.z, max.z)
    );

    public static Color Clamp(this Color v, Color min, Color max) => new(
        Math.Clamp(v.r, min.r, max.r),
        Math.Clamp(v.g, min.g, max.g),
        Math.Clamp(v.b, min.b, max.b),
        Math.Clamp(v.a, min.a, max.a)
    );
    
    public static Vector2Int Negate(this Vector2Int v) => new(
        -v.x,
        -v.y
    );
    
    public static Vector3Int Negate(this Vector3Int v) => new(
        -v.x,
        -v.y,
        -v.z
    );
}
