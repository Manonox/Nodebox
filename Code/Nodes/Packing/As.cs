namespace Nodebox.Nodes;

[RegisterNode]

[Generics([typeof(Vector2), typeof(Vector2Int)])]
[Generics([typeof(Vector2Int), typeof(Vector2)])]

[Generics([typeof(Vector3), typeof(Vector3Int)])]
[Generics([typeof(Vector3Int), typeof(Vector3)])]

[Generics([typeof(Vector3), typeof(Angles)])]
[Generics([typeof(Angles), typeof(Vector3)])]

[Generics([typeof(Vector4), typeof(Rotation)])]
[Generics([typeof(Rotation), typeof(Vector4)])]

[Generics([typeof(Vector4), typeof(Color)])]
[Generics([typeof(Color), typeof(Vector4)])]
public class As<TIn, TOut> : Node
{
	public override Type[] Generics => [typeof(TIn), typeof(TOut)];
    
    public override string Name => $"As<{typeof(TIn).GetPrettyName()}, {typeof(TOut).GetPrettyName()}>";
    public override string Desc => null;
    public override string[] Groups => new string[] { "Packing" }; // Cast?
	public override Vector2 SizeMultiplier => new(1f, 1f);


    const string LETTERS = "XYZW";
    const string COLOR_LETTERS = "RGBA";
    private static Type InVectorBaseType => typeof(TIn).GetVectorBaseType();
    private static Type OutVectorBaseType => typeof(TOut).GetVectorBaseType();
    private static int VectorTypeDimensions => typeof(TIn).GetVectorTypeDimensions();

	public override (Pin[] In, Pin[] Out) InitialPins { get {
        if (!typeof(TIn).IsVectorType())
            throw new NotImplementedException();
        if (!typeof(TOut).IsVectorType())
            throw new NotImplementedException();

        return (
            new Pin[] {
                Pin.New<TIn>(""),
            },
            new Pin[] {
                Pin.New<TOut>(""),
            }
        );
    } }

    public override void Evaluate() {
        if (InVectorBaseType == typeof(float)) {
            var inputs = PackingExtensions.UnpackAnyFloat(GetInput<object>(0));
            if (OutVectorBaseType == typeof(int)) {
                var inputsInt = inputs.Select(x => (int)x);
                SetOutputGeneric(0, TypeLibrary.GetType<TOut>().Create<object>( [.. inputsInt] ));
                return;
            }
            SetOutputGeneric(0, TypeLibrary.GetType<TOut>().Create<object>( [.. inputs] ));
            return;
        }
        
        if (InVectorBaseType == typeof(int)) {
            var inputs = PackingExtensions.UnpackAnyInt(GetInput<object>(0));
            if (OutVectorBaseType == typeof(float)) {
                var inputsFloat = inputs.Select(x => (float)x);
                SetOutputGeneric(0, TypeLibrary.GetType<TOut>().Create<object>( [.. inputsFloat] ));
                return;
            }
            SetOutputGeneric(0, TypeLibrary.GetType<TOut>().Create<object>( [.. inputs] ));
            return;
        }

        throw new NotImplementedException();
    }
}
