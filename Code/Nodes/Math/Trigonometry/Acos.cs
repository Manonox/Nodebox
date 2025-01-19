namespace Nodebox.Nodes;

[RegisterNode]
[Generics(typeof(float))]
[Generics(typeof(double))]
public class Acos<T> : Node
{
	public override Type[] Generics => [typeof(T)];
    
    public override string Name => $"Acos<{typeof(T).GetPrettyName()}>";
    public override string Desc => null;
    public override string[] Groups => [ "Math", "Trigonometry" ];
    public override string[] Aliases => [ "ArcCosine" ];

    public override (Pin[] In, Pin[] Out) InitialPins => (
        new Pin[] {
            Pin.New<T>("X"),
        },
        
        new Pin[] {
            Pin.New<T>("Radians"),
            Pin.New<T>("Degrees"),
        }
    );

    public override void Evaluate() {
        if (typeof(T) == typeof(float)) {
            var rad = MathF.Acos(GetInput<float>(0));
            SetOutput(0, rad);
            SetOutput(1, MathX.RadianToDegree(rad));
            return;
        }

        if (typeof(T) == typeof(double)) {
            var rad = Math.Acos(GetInput<double>(0));
            SetOutput(0, rad);
            SetOutput(1, rad * 180.0 / Math.PI);
            return;
        }

        throw new NotImplementedException();
    }
}
