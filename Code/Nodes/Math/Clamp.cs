namespace Nodebox.Nodes;

[RegisterNode]
[Generics(typeof(float))]
[Generics(typeof(double))]
[Generics(typeof(int))]
//[Generics(typeof(long))]
[Generics(typeof(Vector2))]
[Generics(typeof(Vector3))]
[Generics(typeof(Vector4))]
//[Generics(typeof(Vector2Int))]
//[Generics(typeof(Vector3Int))]
public class Clamp<T> : Node
{
	public override Type[] Generics => [typeof(T)];
    
    public override string Name => $"Clamp<{typeof(T).GetPrettyName()}>";
    public override string Desc => null;
    public override string[] Groups => new string[] { "Math" };

    public override (Pin[] In, Pin[] Out) InitialPins => (
        new Pin[] {
            Pin.New<T>("X"),
            Pin.New<T>("Min"),
            Pin.New<T>("Max"),
        },
        
        new Pin[] {
            Pin.New<T>("Clamped X")
        }
    );

    public override void Evaluate() {
        if (typeof(T) == typeof(float)) {
            SetOutput(0, Math.Clamp(GetInput<float>(0), GetInput<float>(1), GetInput<float>(2)));
            return;
        }
        
        if (typeof(T) == typeof(double)) {
            SetOutput(0, Math.Clamp(GetInput<double>(0), GetInput<double>(1), GetInput<double>(2)));
            return;
        }

        if (typeof(T) == typeof(Vector2)) {
            SetOutput(0, GetInput<Vector2>(0).Clamp(GetInput<Vector2>(1), GetInput<Vector2>(2)));
            return;
        }

        if (typeof(T) == typeof(Vector3)) {
            SetOutput(0, GetInput<Vector3>(0).Clamp(GetInput<Vector3>(1), GetInput<Vector3>(2)));
            return;
        }
        
        if (typeof(T) == typeof(Vector4)) {
            SetOutput(0, GetInput<Vector4>(0).Clamp(GetInput<Vector4>(1), GetInput<Vector4>(2)));
            return;
        }

        if (typeof(T) == typeof(Vector2Int)) {
            SetOutput(0, GetInput<Vector2Int>(0).Clamp(GetInput<Vector2Int>(1), GetInput<Vector2Int>(2)));
            return;
        }

        if (typeof(T) == typeof(Vector3Int)) {
            SetOutput(0, GetInput<Vector3Int>(0).Clamp(GetInput<Vector3Int>(1), GetInput<Vector3Int>(2)));
            return;
        }

        throw new NotImplementedException();
    }
}

