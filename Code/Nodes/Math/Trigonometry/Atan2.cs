namespace Nodebox.Nodes;

[RegisterNode]
[Generics(typeof(float))]
[Generics(typeof(double))]
[Generics(typeof(Vector2))]
public class Atan2<T> : Node
{
	public override Type[] Generics => [typeof(T)];
    
    public override string Name => $"Atan2<{typeof(T).GetPrettyName()}>";
    public override string Desc => null;
    public override string[] Groups => [ "Math", "Trigonometry" ];
    public override string[] Aliases => [ "ArcTangent2" ];

    public override (Pin[] In, Pin[] Out) InitialPins => (
        typeof(T) != typeof(Vector2) ?
            new Pin[] {
                Pin.New<T>("Y"),
                Pin.New<T>("X"),
            }
            :
            new Pin[] {
                Pin.New<T>("XY"),
            },
        
        new Pin[] {
            Pin.New<T>("Radians"),
            Pin.New<T>("Degrees"),
        }
    );

    public override void Evaluate() {
        if (typeof(T) == typeof(float)) {
            var rad = MathF.Atan2(GetInput<float>(0), GetInput<float>(1));
            SetOutput(0, rad);
            SetOutput(1, MathX.RadianToDegree(rad));
            return;
        }

        if (typeof(T) == typeof(double)) {
            var rad = Math.Atan2(GetInput<double>(0), GetInput<double>(1));
            SetOutput(0, rad);
            SetOutput(1, rad * 180.0 / Math.PI);
            return;
        }
        
        if (typeof(T) == typeof(Vector2)) {
            var v = GetInput<Vector2>(0);
            var rad = MathF.Atan2(v.y, v.x);
            SetOutput(0, rad);
            SetOutput(1, MathX.RadianToDegree(rad));
            return;
        }

        throw new NotImplementedException();
    }
}
