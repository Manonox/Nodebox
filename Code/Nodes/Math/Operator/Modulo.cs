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
public class Modulo<T> : Node
{
	public override Type[] Generics => [typeof(T)];
    
    public override string Name => $"Modulo<{typeof(T).GetPrettyName()}>";
    public override string Desc => "Divides A by B and returns the remainder";
    public override string[] Groups => [ "Math", "Operator" ];
    public override string[] Aliases => [ "%", "Remainder" ];

    public override (Pin[] In, Pin[] Out) InitialPins => (
        new Pin[] {
            Pin.New<T>("A"),
            Pin.New<T>("B"),
        },
        
        new Pin[] {
            Pin.New<T>("A%B")
        }
    );

    public override void Evaluate() {
        if (typeof(T) == typeof(float)) {
            SetOutput(0, GetInput<float>(0) % GetInput<float>(1));
            return;
        }
        
        if (typeof(T) == typeof(double)) {
            SetOutput(0, GetInput<double>(0) % GetInput<double>(1));
            return;
        }

        if (typeof(T) == typeof(int)) {
            SetOutput(0, GetInput<int>(0) % GetInput<int>(1));
            return;
        }
        
        if (typeof(T) == typeof(long)) {
            SetOutput(0, GetInput<long>(0) % GetInput<long>(1));
            return;
        }

        if (typeof(T) == typeof(Vector2)) {
            SetOutput(0, GetInput<Vector2>(0).Modulo(GetInput<Vector2>(1)));
            return;
        }

        if (typeof(T) == typeof(Vector3)) {
            SetOutput(0, GetInput<Vector3>(0).Modulo(GetInput<Vector3>(1)));
            return;
        }
        
        if (typeof(T) == typeof(Vector4)) {
            SetOutput(0, GetInput<Vector4>(0).Modulo(GetInput<Vector4>(1)));
            return;
        }

        if (typeof(T) == typeof(Vector2Int)) {
            SetOutput(0, GetInput<Vector2Int>(0).Modulo(GetInput<Vector2Int>(1)));
            return;
        }
        
        if (typeof(T) == typeof(Vector3Int)) {
            SetOutput(0, GetInput<Vector3Int>(0).Modulo(GetInput<Vector3Int>(1)));
            return;
        }

        throw new NotImplementedException();
    }
}

