namespace Nodebox.Nodes;

[RegisterNode]
[Generics(typeof(float))]
[Generics(typeof(double))]
public class Cos<T> : Node
{
	public override Type[] Generics => [typeof(T)];
    
    public override string Name => $"Cos<{typeof(T).GetPrettyName()}>";
    public override string Desc => null;
    public override string[] Groups => [ "Math", "Trigonometry" ];
    public override string[] Aliases => [ "Cosine" ];

    public override (Pin[] In, Pin[] Out) InitialPins => (
        new Pin[] {
            Pin.New<T>("X"),
        },
        
        new Pin[] {
            Pin.New<T>("From Radians"),
            Pin.New<T>("From Degrees"),
        }
    );

    public override void Evaluate() {
        if (typeof(T) == typeof(float)) {
            SetOutput(0, MathF.Cos(GetInput<float>(0)));
            SetOutput(1, MathF.Cos(MathX.DegreeToRadian(GetInput<float>(0))));
            return;
        }

        if (typeof(T) == typeof(double)) {
            SetOutput(0, Math.Cos(GetInput<double>(0)));
            SetOutput(1, Math.Cos(GetInput<double>(0) * Math.PI / 180.0));
            return;
        }

        throw new NotImplementedException();
    }
}
