using System.Numerics;

namespace Nodebox;

public static class TypeExtensions {
    public static string GetPrettyName(this Type type)
    {
        if (type == typeof(float)) {
            return "float";
        }
        else if (type == typeof(double)) {
            return "double";
        }
        else if (type == typeof(int)) {
            return "int";
        }
        else if (type == typeof(string)) {
            return "string";
        }
        else if (type == typeof(bool)) {
            return "bool";
        }
        else if (type == typeof(Vector2Int)) {
            return "Vector2i";
        }
        else if (type == typeof(Vector3Int)) {
            return "Vector3i";
        }
        else {
            return TypeLibrary.GetType(type).Title.Replace(" ", null);
        }
    }

    public static bool IsVectorType<T>() => typeof(T).IsVectorType();

    public static bool IsVectorType(this Type type)
    {
        if (type == typeof(Vector2))
            return true;
        if (type == typeof(Vector3))
            return true;
        if (type == typeof(Vector4))
            return true;
        if (type == typeof(Vector2Int))
            return true;
        if (type == typeof(Vector3Int))
            return true;
        if (type == typeof(Angles))
            return true;
        if (type == typeof(Rotation))
            return true;
        if (type == typeof(Quaternion))
            return true;
        if (type == typeof(Color))
            return true;
        return false;
    }

	public static Type GetVectorBaseType<T>() => typeof(T).GetVectorBaseType();
	public static Type GetVectorBaseType(this Type type) => type switch {
		_ when type == typeof(Vector2) => typeof(float),
		_ when type == typeof(Vector3) => typeof(float),
		_ when type == typeof(Vector4) => typeof(float),
		_ when type == typeof(Vector2Int) => typeof(int),
		_ when type == typeof(Vector3Int) => typeof(int),
		_ when type == typeof(Angles) => typeof(float),
		_ when type == typeof(Rotation) => typeof(float),
		_ when type == typeof(Quaternion) => typeof(float),
		_ when type == typeof(Color) => typeof(float),
		_ => throw new ArgumentException("Argument type is not a vector type")
	};

	public static int GetVectorTypeDimensions<T>() => typeof(T).GetVectorTypeDimensions();
    public static int GetVectorTypeDimensions(this Type type) => type switch {
		_ when type == typeof(Vector2) => 2,
		_ when type == typeof(Vector2Int) => 2,
		_ when type == typeof(Vector3) => 3,
		_ when type == typeof(Vector3Int) => 3,
		_ when type == typeof(Vector4) => 4,
		_ when type == typeof(Angles) => 3,
		_ when type == typeof(Rotation) => 4,
		_ when type == typeof(Quaternion) => 4,
		_ when type == typeof(Color) => 4,
		_ => 1 // throw new ArgumentException("Argument type is not a vector type")
	};
}
