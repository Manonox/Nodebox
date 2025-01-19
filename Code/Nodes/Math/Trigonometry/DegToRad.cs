namespace Nodebox.Nodes;

[RegisterNode]
[Generics(typeof(float))]
[Generics(typeof(double))]
public class DegToRad<T> : Node
{
	public override Type[] Generics => [typeof(T)];
    
    public override string Name => $"DegToRad<{typeof(T).GetPrettyName()}>";
    public override string Desc => "Converts degrees to radians";
    public override string[] Groups => [ "Math", "Trigonometry" ];
    public override string[] Aliases => [ "Deg2Rad", "DegreesToRadians", "Degrees2Radians" ];

    public override (Pin[] In, Pin[] Out) InitialPins => (
        new Pin[] {
            Pin.New<T>("Degrees"),
        },
        
        new Pin[] {
            Pin.New<T>("Radians"),
        }
    );

    public override void Evaluate() {
        if (typeof(T) == typeof(float)) {
            SetOutput(0, MathX.DegreeToRadian(GetInput<float>(0)));
            return;
        }

        if (typeof(T) == typeof(double)) {
            SetOutput(0, GetInput<float>(0) * Math.PI / 180.0);
            return;
        }

        throw new NotImplementedException();
    }
}
