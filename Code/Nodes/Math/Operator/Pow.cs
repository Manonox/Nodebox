namespace Nodebox.Nodes;

[RegisterNode]
[Generics(typeof(float))]
[Generics(typeof(double))]
[Generics(typeof(int))]
// [Generics(typeof(long))]
public class Power<T> : Node
{
	public override Type[] Generics => [typeof(T)];
    
    public override string Name => $"Power<{typeof(T).GetPrettyName()}>";
    public override string Desc => "Raises A to the power of B";
    public override string[] Groups => [ "Math", "Operator" ];
    public override string[] Aliases => [ "^", "**", "Exponent" ];

    public override (Pin[] In, Pin[] Out) InitialPins => (
        new Pin[] {
            Pin.New<T>("A"),
            new(typeof(T) == typeof(long) ? typeof(int) : typeof(T), "B"),
        },
        
        new Pin[] {
            Pin.New<T>("A pow B")
        }
    );

    public override void Evaluate() {
        if (typeof(T) == typeof(float)) {
            SetOutput(0, MathF.Pow(GetInput<float>(0), GetInput<float>(1)));
            return;
        }
        
        if (typeof(T) == typeof(double)) {
            SetOutput(0, Math.Pow(GetInput<double>(0), GetInput<double>(1)));
            return;
        }

        if (typeof(T) == typeof(int)) {
            SetOutput(0, GetInput<int>(0).Pow(GetInput<int>(1)));
            return;
        }
        
        if (typeof(T) == typeof(long)) {
            SetOutput(0, GetInput<long>(0).Pow(GetInput<int>(1)));
            return;
        }

        throw new NotImplementedException();
    }
}

