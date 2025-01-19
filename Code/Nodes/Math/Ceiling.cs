namespace Nodebox.Nodes;

[RegisterNode]
[Generics(typeof(float))]
[Generics(typeof(double))]
[Generics(typeof(Vector2))]
[Generics(typeof(Vector3))]
[Generics(typeof(Vector4))]
public class Ceiling<T> : Node
{
	public override Type[] Generics => [typeof(T)];
    
    public override string Name => $"Ceiling<{typeof(T).GetPrettyName()}>";
    public override string Desc => "Rounds the value up";
    public override string[] Groups => new string[] { "Math" };

    public override (Pin[] In, Pin[] Out) InitialPins => (
        new Pin[] {
            Pin.New<T>("X"),
        },
        
        new Pin[] {
            Pin.New<T>("⌈X⌉")
        }
    );

    public override void Evaluate() {
        if (typeof(T) == typeof(float)) {
            SetOutput(0, MathF.Ceiling(GetInput<float>(0)));
            return;
        }
        
        if (typeof(T) == typeof(double)) {
            SetOutput(0, Math.Ceiling(GetInput<double>(0)));
            return;
        }

        if (typeof(T) == typeof(Vector2)) {
            SetOutput(0, GetInput<Vector2>(0).Ceiling());
            return;
        }

        if (typeof(T) == typeof(Vector3)) {
            SetOutput(0, GetInput<Vector3>(0).Ceiling());
            return;
        }
        
        if (typeof(T) == typeof(Vector4)) {
            SetOutput(0, GetInput<Vector4>(0).Ceiling());
            return;
        }

        throw new NotImplementedException();
    }
}

