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

public class Divide<T> : Node
{
	public override Type[] Generics => [typeof(T)];
    
    public override string Name => $"Divide<{typeof(T).GetPrettyName()}>";
    public override string Desc => null;
    public override string[] Groups => new string[] { "Math", "Operator" };
    public override string[] Aliases => [ "/" ];

    public override (Pin[] In, Pin[] Out) InitialPins => (
        new Pin[] {
            Pin.New<T>("A"),
            Pin.New<T>("B"),
        },
        
        new Pin[] {
            Pin.New<T>("A/B")
        }
    );

    public override void Evaluate() {
        if (typeof(T) == typeof(float)) {
            SetOutput(0, GetInput<float>(0) / GetInput<float>(1));
            return;
        }
        
        if (typeof(T) == typeof(double)) {
            SetOutput(0, GetInput<double>(0) / GetInput<double>(1));
            return;
        }

        if (typeof(T) == typeof(int)) {
            SetOutput(0, GetInput<int>(0) / GetInput<int>(1));
            return;
        }
        
        if (typeof(T) == typeof(long)) {
            SetOutput(0, GetInput<long>(0) / GetInput<long>(1));
            return;
        }

        if (typeof(T) == typeof(Vector2)) {
            SetOutput(0, GetInput<Vector2>(0) / GetInput<Vector2>(1));
            return;
        }

        if (typeof(T) == typeof(Vector3)) {
            SetOutput(0, GetInput<Vector3>(0) / GetInput<Vector3>(1));
            return;
        }
        
        if (typeof(T) == typeof(Vector4)) {
            var a = GetInput<Vector4>(0);
            var b = GetInput<Vector4>(1);
            SetOutput(0, new Vector4(a.x / b.x, a.y / b.y, a.z / b.z, a.w / b.w));
            return;
        }

        if (typeof(T) == typeof(Vector2Int)) {
            SetOutput(0, GetInput<Vector2Int>(0) / GetInput<Vector2Int>(1));
            return;
        }
        
        if (typeof(T) == typeof(Vector3Int)) {
            SetOutput(0, GetInput<Vector3Int>(0) / GetInput<Vector3Int>(1));
            return;
        }

        throw new NotImplementedException();
    }
}

