namespace Nodebox.Nodes;

[RegisterNode]
[Generics(typeof(float))]
[Generics(typeof(double))]
public class Tan<T> : Node
{
	public override Type[] Generics => [typeof(T)];
    
    public override string Name => $"Tan<{typeof(T).GetPrettyName()}>";
    public override string Desc => null;
    public override string[] Groups => [ "Math", "Trigonometry" ];
    public override string[] Aliases => [ "Tangent" ];

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
            SetOutput(0, MathF.Tan(GetInput<float>(0)));
            SetOutput(1, MathF.Tan(MathX.DegreeToRadian(GetInput<float>(0))));
            return;
        }

        if (typeof(T) == typeof(double)) {
            SetOutput(0, Math.Tan(GetInput<double>(0)));
            SetOutput(0, Math.Tan(GetInput<double>(0) * Math.PI / 180.0));
            return;
        }

        throw new NotImplementedException();
    }
}

