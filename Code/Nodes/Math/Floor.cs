namespace Nodebox.Nodes;

[RegisterNode]
[Generics(typeof(float))]
[Generics(typeof(double))]
[Generics(typeof(Vector2))]
[Generics(typeof(Vector3))]
[Generics(typeof(Vector4))]
public class Floor<T> : Node
{
	public override Type[] Generics => [typeof(T)];
    
    public override string Name => $"Floor<{typeof(T).GetPrettyName()}>";
    public override string Desc => "Rounds the value down";
    public override string[] Groups => new string[] { "Math" };

    public override (Pin[] In, Pin[] Out) InitialPins => (
        new Pin[] {
            Pin.New<T>("A"),
        },
        
        new Pin[] {
            Pin.New<T>("⌊A⌋")
        }
    );

    public override void Evaluate() {
        if (typeof(T) == typeof(float)) {
            SetOutput(0, MathF.Floor(GetInput<float>(0)));
            return;
        }
        
        if (typeof(T) == typeof(double)) {
            SetOutput(0, Math.Floor(GetInput<double>(0)));
            return;
        }

        if (typeof(T) == typeof(Vector2)) {
            SetOutput(0, GetInput<Vector2>(0).Floor());
            return;
        }

        if (typeof(T) == typeof(Vector3)) {
            SetOutput(0, GetInput<Vector3>(0).Floor());
            return;
        }
        
        if (typeof(T) == typeof(Vector4)) {
            SetOutput(0, GetInput<Vector4>(0).Floor());
            return;
        }

        throw new NotImplementedException();
    }
}

