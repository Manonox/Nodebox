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
public class Unpack<T> : Node
{
	public override Type[] Generics => [typeof(T)];
    
    public override string Name => $"Unpack<{typeof(T).GetPrettyName()}>";
    public override string Desc => "Takes a vector and returns it's components";
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
        var outputPins = Enumerable.Range(0, VectorTypeDimensions)
            .Select(index => new Pin(VectorBaseType, letters[index].ToString())).ToArray();

        return (
            new Pin[] {
                Pin.New<T>("V"),
            },

            outputPins
        );
    } }

    public override void Evaluate() {
        if (!typeof(T).IsVectorType())
            throw new NotImplementedException();

        if (typeof(T) == typeof(Vector2)) {
            GetInput<Vector2>(0)
                .Unpack()
                .Enumerate()
                .ForEach(x => SetOutput(x.Index, x.Item));
            return;
        }
        
        if (typeof(T) == typeof(Vector3)) {
            GetInput<Vector3>(0)
                .Unpack()
                .Enumerate()
                .ForEach(x => SetOutput(x.Index, x.Item));
            return;
        }

        if (typeof(T) == typeof(Vector4)) {
            GetInput<Vector4>(0)
                .Unpack()
                .Enumerate()
                .ForEach(x => SetOutput(x.Index, x.Item));
            return;
        }

        if (typeof(T) == typeof(Vector2Int)) {
            GetInput<Vector2Int>(0)
                .Unpack()
                .Enumerate()
                .ForEach(x => SetOutput(x.Index, x.Item));
            return;
        }

        if (typeof(T) == typeof(Vector3Int)) {
            GetInput<Vector3Int>(0)
                .Unpack()
                .Enumerate()
                .ForEach(x => SetOutput(x.Index, x.Item));
            return;
        }
        
        if (typeof(T) == typeof(Angles)) {
            GetInput<Angles>(0)
                .Unpack()
                .Enumerate()
                .ForEach(x => SetOutput(x.Index, x.Item));
            return;
        }
        
        if (typeof(T) == typeof(Rotation)) {
            GetInput<Rotation>(0)
                .Unpack()
                .Enumerate()
                .ForEach(x => SetOutput(x.Index, x.Item));
            return;
        }

        if (typeof(T) == typeof(Color)) {
            GetInput<Color>(0)
                .Unpack()
                .Enumerate()
                .ForEach(x => SetOutput(x.Index, x.Item));
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
public class Unpack<TOut0, TOut1> : Node
{
	public override Type[] Generics => [typeof(TOut0), typeof(TOut1)];
    
    public override string Name => $"Unpack<{typeof(TOut0).GetPrettyName()}, {typeof(TOut1).GetPrettyName()}>";
    public override string Desc => "I don't even fucking know anymore";
    public override string[] Groups => new string[] { "Packing" };
	public override Vector2 SizeMultiplier => new(0.66f, 1f);


    const string LETTERS = "XYZW";
    private static Type VectorBaseType => typeof(TOut0).GetVectorBaseType();
    private static int In0VectorTypeDimensions => typeof(TOut0).GetVectorTypeDimensions();
    private static int In1VectorTypeDimensions => typeof(TOut1).GetVectorTypeDimensions();
    private static Type OutType { get {
        if (typeof(TOut0) == typeof(Vector2)) {
            if (typeof(TOut1) == typeof(float)) {
                return typeof(Vector3);
            }

            if (typeof(TOut1) == typeof(Vector2)) {
                return typeof(Vector3);
            }
        }

        if (typeof(TOut0) == typeof(Vector3)) {
            if (typeof(TOut1) == typeof(float)) {
                return typeof(Vector4);
            }
        }

        if (typeof(TOut0) == typeof(Vector2Int)) {
            if (typeof(TOut1) == typeof(int)) {
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
                new(OutType, "V"),
            },
            
            new Pin[] {
                Pin.New<TOut0>(name0),
                Pin.New<TOut1>(name1),
            }
        );
    } }

    public override void Evaluate() {
        if (typeof(TOut0) == typeof(Vector2) && typeof(TOut1) == typeof(float)) {
            var seq = GetInput<Vector3>(0).Unpack();
            SetOutput(0, seq.PackVector2());
            SetOutput(1, seq.Skip(2).First());
            return;
        }

        if (typeof(TOut0) == typeof(Vector3) && typeof(TOut1) == typeof(float)) {
            var seq = GetInput<Vector4>(0).Unpack();
            SetOutput(0, seq.PackVector3());
            SetOutput(1, seq.Skip(3).First());
            return;
        }
        
        if (typeof(TOut0) == typeof(Vector2) && typeof(TOut1) == typeof(Vector2)) {
            var seq = GetInput<Vector4>(0).Unpack();
            SetOutput(0, seq.PackVector2());
            SetOutput(1, seq.Skip(2).PackVector2());
            return;
        }
        
        if (typeof(TOut0) == typeof(Vector2Int) && typeof(TOut1) == typeof(int)) {
            var seq = GetInput<Vector3Int>(0).Unpack();
            SetOutput(0, seq.PackVector2Int());
            SetOutput(1, seq.Skip(2).First());
            return;
        }

        throw new NotImplementedException();
    }
}

