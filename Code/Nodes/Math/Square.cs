namespace Nodebox.Nodes;

public sealed class Square<T> : Node
{
    public override string Name => "Square";
    public override string Desc => "Takes a number X and returns it's square";
    public override string[] Groups => new string[] { "Math", "Operator" };

    public override (Pin[] In, Pin[] Out) InitialPins => (
        new Pin[] {
            new Pin<T>("X")
        },
        
        new Pin[] {
            new Pin<T>("X²")
        }
    );

    public override void Evaluate() {
        if (typeof(T) == typeof(float)) {
            var x = GetInput<float>(0);
            SetOutput(0, x * x);
            return;
        }

        if (typeof(T) == typeof(double)) {
            var x = GetInput<double>(0);
            SetOutput(0, x * x);
            return;
        }
    }
}

