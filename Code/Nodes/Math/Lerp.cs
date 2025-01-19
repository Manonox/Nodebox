namespace Nodebox.Nodes;

[RegisterNode]
[Generics(typeof(float))]
[Generics(typeof(double))]
[Generics(typeof(Vector2))]
[Generics(typeof(Vector3))]
[Generics(typeof(Vector4))]
public class Lerp<T> : Node
{
	public override Type[] Generics => [typeof(T)];
    
    public override string Name => $"Lerp<{typeof(T).GetPrettyName()}>";
    public override string Desc => null;
    public override string[] Groups => new string[] { "Math" };
    public override string[] Aliases => new string[] { "Interpolate", "Interpolation" };

    public override (Pin[] In, Pin[] Out) InitialPins => (
        new Pin[] {
            Pin.New<T>("From"),
            Pin.New<T>("To"),
            new(typeof(T) == typeof(double) ? typeof(double) : typeof(float), "Frac"),
        },
        
        new Pin[] {
            Pin.New<T>("*"),
        }
    );

    public override void Evaluate() {
        if (typeof(T) == typeof(float)) {
            SetOutput(0, GetInput<float>(0).LerpTo(GetInput<float>(1), GetInput<float>(2)));
            return;
        }
        
        if (typeof(T) == typeof(double)) {
            SetOutput(0, GetInput<double>(0).LerpTo(GetInput<double>(1), GetInput<double>(2)));
            return;
        }

        if (typeof(T) == typeof(Vector2)) {
            SetOutput(0, GetInput<Vector2>(0).LerpTo(GetInput<Vector2>(1), GetInput<float>(2)));
            return;
        }

        if (typeof(T) == typeof(Vector3)) {
            SetOutput(0, GetInput<Vector3>(0).LerpTo(GetInput<Vector3>(1), GetInput<float>(2)));
            return;
        }
        
        if (typeof(T) == typeof(Vector4)) {
            SetOutput(0, GetInput<Vector4>(0).LerpTo(GetInput<Vector4>(1), GetInput<float>(2)));
            return;
        }

        throw new NotImplementedException();
    }
}


[RegisterNode]
[Generics(typeof(float))]
[Generics(typeof(double))]
[Generics(typeof(Vector2))]
[Generics(typeof(Vector3))]
[Generics(typeof(Vector4))]
public class LerpUnclamped<T> : Lerp<T>
{
    public override string Name => $"LerpUnclamped<{typeof(T).GetPrettyName()}>";

    public override void Evaluate() {
        if (typeof(T) == typeof(float)) {
            SetOutput(0, GetInput<float>(0).LerpTo(GetInput<float>(1), GetInput<float>(2), false));
            return;
        }
        
        if (typeof(T) == typeof(double)) {
            SetOutput(0, GetInput<double>(0).LerpTo(GetInput<double>(1), GetInput<double>(2), false));
            return;
        }

        if (typeof(T) == typeof(Vector2)) {
            SetOutput(0, GetInput<Vector2>(0).LerpTo(GetInput<Vector2>(1), GetInput<float>(2), false));
            return;
        }

        if (typeof(T) == typeof(Vector3)) {
            SetOutput(0, GetInput<Vector3>(0).LerpTo(GetInput<Vector3>(1), GetInput<float>(2), false));
            return;
        }
        
        if (typeof(T) == typeof(Vector4)) {
            SetOutput(0, GetInput<Vector4>(0).LerpTo(GetInput<Vector4>(1), GetInput<float>(2), false));
            return;
        }

        throw new NotImplementedException();
    }
}



