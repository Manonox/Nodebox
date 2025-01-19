namespace Nodebox.Nodes;

[RegisterNode]
[Generics(typeof(float))]
[Generics(typeof(double))]
public class RadToDeg<T> : Node
{
	public override Type[] Generics => [typeof(T)];
    
    public override string Name => $"RadToDeg<{typeof(T).GetPrettyName()}>";
    public override string Desc => "Converts radians to degrees";
    public override string[] Groups => [ "Math", "Trigonometry" ];
    public override string[] Aliases => [ "Rad2Deg", "RadiansToDegrees", "Radians2Degrees" ];

    public override (Pin[] In, Pin[] Out) InitialPins => (
        new Pin[] {
            Pin.New<T>("Radians"),
        },
        
        new Pin[] {
            Pin.New<T>("Degrees"),
        }
    );

    public override void Evaluate() {
        if (typeof(T) == typeof(float)) {
            SetOutput(0, MathX.RadianToDegree(GetInput<float>(0)));
            return;
        }

        if (typeof(T) == typeof(double)) {
            SetOutput(0, GetInput<float>(0) * 180.0 / Math.PI);
            return;
        }

        throw new NotImplementedException();
    }
}
