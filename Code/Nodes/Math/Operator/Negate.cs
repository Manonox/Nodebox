namespace Nodebox.Nodes;

[RegisterNode]
[Generics(typeof(float))]
[Generics(typeof(double))]
[Generics(typeof(int))]
// [Generics(typeof(long))]
[Generics(typeof(Vector2))]
[Generics(typeof(Vector3))]
[Generics(typeof(Vector4))]
// [Generics(typeof(Vector2Int))]
// [Generics(typeof(Vector3Int))]

[Generics(typeof(Angles))]

public class Negate<T> : Node
{
	public override Type[] Generics => [typeof(T)];
    
    public override string Name => $"Negate<{typeof(T).GetPrettyName()}>";
    public override string Desc => "Returns the value with the sign flipped";
    public override string[] Groups => new string[] { "Math", "Operator" };
    public override string[] Aliases => [ "-" ];

    public override (Pin[] In, Pin[] Out) InitialPins => (
        new Pin[] {
            Pin.New<T>("X"),
        },
        
        new Pin[] {
            Pin.New<T>(!typeof(T).IsVectorType() ? "|X|" : "Positive X")
        }
    );

    public override void Evaluate() {
        if (typeof(T) == typeof(float)) {
            SetOutput(0, -GetInput<float>(0));
            return;
        }
        
        if (typeof(T) == typeof(double)) {
            SetOutput(0, -GetInput<double>(0));
            return;
        }

        if (typeof(T) == typeof(int)) {
            SetOutput(0, -GetInput<int>(0));
            return;
        }

        if (typeof(T) == typeof(long)) {
            SetOutput(0, -GetInput<long>(0));
            return;
        }
        
        if (typeof(T) == typeof(Vector2)) {
            SetOutput(0, -GetInput<Vector2>(0));
            return;
        }
        
        if (typeof(T) == typeof(Vector3)) {
            SetOutput(0, -GetInput<Vector3>(0));
            return;
        }
        
        if (typeof(T) == typeof(Vector4)) {
            SetOutput(0, -GetInput<Vector4>(0));
            return;
        }
        
        if (typeof(T) == typeof(Vector2Int)) {
            SetOutput(0, GetInput<Vector2Int>(0).Negate());
            return;
        }
        
        if (typeof(T) == typeof(Vector3Int)) {
            SetOutput(0, GetInput<Vector3Int>(0).Negate());
            return;
        }

        if (typeof(T) == typeof(Angles)) {
            SetOutput(0, GetInput<Angles>(0).Negate());
            return;
        }

        throw new NotImplementedException();
    }
}

