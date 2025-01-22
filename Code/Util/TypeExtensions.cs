namespace Nodebox;


public static class TypeExtensions {
	public static T CreateClosedGeneric<T>(this Type type, object[] args) { // params
        var typeDescription = TypeLibrary.GetType(type);
        if (!typeDescription.IsGenericType) {
            return typeDescription.Create<T>(args);
        }

        var generics = TypeLibrary.GetGenericArguments(type);
        return typeDescription.CreateGeneric<T>(generics, args);
    }

	private abstract record Dummy<T> { }
	public static string GetDisplayName(this Type type)
	{
		if (type == typeof(Vector2))
			return "Vector2";
		if (type == typeof(Vector3))
			return "Vector3";
		if (type == typeof(Vector4))
			return "Vector4";
		if (type == typeof(Vector2Int))
			return "Vector2Int";
		if (type == typeof(Vector3Int))
			return "Vector3Int";
		if (type == typeof(Rotation))
			return "Rotation";
		if (type == typeof(Angles))
			return "Angles";
		if (type == typeof(Color))
			return "Color";

		var name = DisplayInfo.ForType(type).Name;
		if (!TypeLibrary.GetType(type).IsGenericType) {
			return name;
		}

		Type[] generics;
		try {
			generics = TypeLibrary.GetGenericArguments(type);
		}
		catch {
			generics = TypeLibrary.GetType(type).GenericArguments;
			return $"{name}<{string.Join(", ", generics.Enumerate().Select(x => "T" + x.Index.ToString()))}>";
		}

		Assert.True(generics.Length > 0);

		return $"{name}<{string.Join(", ", generics.Select(x => x.GetDisplayName()))}>";
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
        // if (type == typeof(Quaternion))
        //     return true;
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
		// _ when type == typeof(Quaternion) => typeof(float),
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
		// _ when type == typeof(Quaternion) => 4,
		_ when type == typeof(Color) => 4,
		_ => 1 // throw new ArgumentException("Argument type is not a vector type")
	};
}
