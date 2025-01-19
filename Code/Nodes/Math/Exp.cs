namespace Nodebox.Nodes;

[RegisterNode]
[Generics(typeof(float))]
[Generics(typeof(double))]
//[Generics(typeof(int))]
//[Generics(typeof(long))]
public class Exp<T> : Node
{
	public override Type[] Generics => [typeof(T)];
    
    public override string Name => $"Exp<{typeof(T).GetPrettyName()}>";
    public override string Desc => "Constant E to the power of X";
    public override string[] Groups => new string[] { "Math" };

    public override (Pin[] In, Pin[] Out) InitialPins => (
        new Pin[] {
            Pin.New<T>("X")
        },
        
        new Pin[] {
            Pin.New<T>("E to the X")
        }
    );

    public override void Evaluate() {
        if (typeof(T) == typeof(float)) {
            SetOutput(0, MathF.Exp(GetInput<float>(0)));
            return;
        }

        if (typeof(T) == typeof(double)) {
            SetOutput(0, Math.Exp(GetInput<double>(0)));
            return;
        }

        throw new NotImplementedException();
    }
}

