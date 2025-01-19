namespace Nodebox.Nodes;

[RegisterNode]
[Generics(typeof(Vector2))]
[Generics(typeof(Vector3))]
[Generics(typeof(Vector4))]
[Generics(typeof(Vector2Int))]
[Generics(typeof(Vector3Int))]

[Generics(typeof(Angles))]
[Generics(typeof(Rotation))]

[Generics(typeof(Color))]
public class Pack<T> : Node
{
	public override Type[] Generics => [typeof(T)];
    
    public override string Name => $"Pack<{typeof(T).GetPrettyName()}>";
    public override string Desc => "Takes components of a vector type and returns the resulting vector";
    public override string[] Groups => new string[] { "Packing" };
	public override Vector2 SizeMultiplier => new(0.66f, 1f);


    const string LETTERS = "XYZW";
    const string COLOR_LETTERS = "RGBA";
    private static Type VectorBaseType => typeof(T).GetVectorBaseType();
    private static int VectorTypeDimensions => typeof(T).GetVectorTypeDimensions();

	public override (Pin[] In, Pin[] Out) InitialPins { get {
        if (!typeof(T).IsVectorType())
            throw new NotImplementedException();

        var letters = typeof(T) != typeof(Color) ? LETTERS : COLOR_LETTERS;
        var inputPins = Enumerable.Range(0, VectorTypeDimensions)
            .Select(index => new Pin(VectorBaseType, letters[index].ToString())).ToArray();

        return (
            inputPins,
            
            new Pin[] {
                Pin.New<T>("V"),
            }
        );
    } }

    public override void Evaluate() {
        if (!typeof(T).IsVectorType())
            throw new NotImplementedException();

        if (VectorBaseType == typeof(float)) {
            var inputPins = Enumerable.Range(0, VectorTypeDimensions)
                .Select(index => (object)GetInput<float>(index));
            SetOutputGeneric(0, TypeLibrary.GetType<T>().Create<object>( [.. inputPins] ));
            return;
        }
        
        if (VectorBaseType == typeof(int)) {
            var inputPins = Enumerable.Range(0, VectorTypeDimensions)
                .Select(index => (object)GetInput<int>(index));
            SetOutputGeneric(0, TypeLibrary.GetType<T>().Create<object>( [.. inputPins] ));
            return;
        }

        throw new NotImplementedException();
    }
}


[RegisterNode]
[Generics([typeof(Vector2), typeof(float)])]
[Generics([typeof(Vector3), typeof(float)])]
[Generics([typeof(Vector2), typeof(Vector2)])]
[Generics([typeof(Vector2Int), typeof(int)])]
public class Pack<TIn0, TIn1> : Node
{
	public override Type[] Generics => [typeof(TIn0), typeof(TIn1)];
    
    public override string Name => $"Pack<{typeof(TIn0).GetPrettyName()}, {typeof(TIn1).GetPrettyName()}>";
    public override string Desc => "I don't even fucking know anymore";
    public override string[] Groups => new string[] { "Packing" };
	public override Vector2 SizeMultiplier => new(0.66f, 1f);


    const string LETTERS = "XYZW";
    private static Type VectorBaseType => typeof(TIn0).GetVectorBaseType();
    private static int In0VectorTypeDimensions => typeof(TIn0).GetVectorTypeDimensions();
    private static int In1VectorTypeDimensions => typeof(TIn1).GetVectorTypeDimensions();
    private static Type OutType { get {
        if (typeof(TIn0) == typeof(Vector2)) {
            if (typeof(TIn1) == typeof(float)) {
                return typeof(Vector3);
            }

            if (typeof(TIn1) == typeof(Vector2)) {
                return typeof(Vector4);
            }
        }

        if (typeof(TIn0) == typeof(Vector3)) {
            if (typeof(TIn1) == typeof(float)) {
                return typeof(Vector4);
            }
        }

        if (typeof(TIn0) == typeof(Vector2Int)) {
            if (typeof(TIn1) == typeof(int)) {
                return typeof(Vector3Int);
            }
        }

        throw new NotImplementedException();
    } }

	public override (Pin[] In, Pin[] Out) InitialPins { get {
        var name0 = Enumerable.Range(0, In0VectorTypeDimensions)
            .Select(x => LETTERS[x].ToString())
            .Aggregate((a, b) => a + b);
        var name1 = Enumerable.Range(In0VectorTypeDimensions, In1VectorTypeDimensions)
            .Select(x => LETTERS[x].ToString())
            .Aggregate((a, b) => a + b);

        if (OutType == null)
            throw new NotImplementedException();

        return (
            new Pin[] {
                Pin.New<TIn0>(name0),
                Pin.New<TIn1>(name1),
            },
            
            new Pin[] {
                new(OutType, "V"),
            }
        );
    } }

    public override void Evaluate() {
        if (typeof(TIn0) == typeof(Vector2) && typeof(TIn1) == typeof(float)) {
            SetOutput(0, GetInput<Vector2>(0).Unpack().Append(GetInput<float>(1)).PackVector3());
            return;
        }

        if (typeof(TIn0) == typeof(Vector3) && typeof(TIn1) == typeof(float)) {
            SetOutput(0, GetInput<Vector3>(0).Unpack().Append(GetInput<float>(1)).PackVector4());
            return;
        }
        
        if (typeof(TIn0) == typeof(Vector2) && typeof(TIn1) == typeof(Vector2)) {
            SetOutput(0, GetInput<Vector2>(0).Unpack().Concat(GetInput<Vector2>(1).Unpack()).PackVector4());
            return;
        }
        
        if (typeof(TIn0) == typeof(Vector2Int) && typeof(TIn1) == typeof(int)) {
            SetOutput(0, GetInput<Vector2Int>(0).Unpack().Append(GetInput<int>(1)).PackVector3Int());
            return;
        }

        throw new NotImplementedException();
    }
}

