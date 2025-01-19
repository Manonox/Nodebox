namespace Nodebox.Nodes;

[RegisterNode]
[Generics(typeof(float))]
[Generics(typeof(double))]
public class DeltaAngle<T> : Node
{
	public override Type[] Generics => [typeof(T)];
    
    public override string Name => $"DeltaAngle<{typeof(T).GetPrettyName()}>";
    public override string Desc => "Gets the difference between 2 angles (just degrees or radians, not the `Angles`)";
    public override string[] Groups => [ "Math", "Trigonometry" ];
    public override string[] Aliases => [ "DifferenceAngle" ];

    public override (Pin[] In, Pin[] Out) InitialPins => (
        new Pin[] {
            Pin.New<T>("From"),
            Pin.New<T>("To"),
        },
        
        new Pin[] {
            Pin.New<T>("Radians"),
            Pin.New<T>("Degrees"),
        }
    );

    public static double UnsignedMod(double a, double b)
    {
        return a - b * Math.Floor(a / b);
    }

    private static double DeltaRadians(double from, double to) {
        double num = UnsignedMod(to - from, Math.PI * 2.0);
        if (!(num >= Math.PI))
        {
            return num;
        }

        return num - Math.PI * 2f;
    }

    public static double DeltaDegrees(double from, double to) {
        double num = UnsignedMod(to - from, 360f);
        if (!(num >= 180.0))
        {
            return num;
        }

        return num - 360.0;
    }

    public override void Evaluate() {
        if (typeof(T) == typeof(float)) {
            
            SetOutput(0, MathX.DeltaRadians(GetInput<float>(0), GetInput<float>(1)));
            SetOutput(1, MathX.DeltaDegrees(GetInput<float>(0), GetInput<float>(1)));
            return;
        }

        if (typeof(T) == typeof(double)) {
            SetOutput(0, DeltaRadians(GetInput<double>(0), GetInput<double>(1)));
            SetOutput(1, DeltaDegrees(GetInput<double>(0), GetInput<double>(1)));
            return;
        }

        throw new NotImplementedException();
    }
}
