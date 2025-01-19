namespace Nodebox.Nodes;

[RegisterNode]
[Generics(typeof(float))]
[Generics(typeof(double))]
[Generics(typeof(int))]
//[Generics(typeof(long))]
public class Square<T> : Node
{
	public override Type[] Generics => [typeof(T)];
    
    public override string Name => $"Square<{typeof(T).GetPrettyName()}>";
    public override string Desc => "Takes a number and multiplies it by itself";
    public override string[] Groups => new string[] { "Math" };
	public override Vector2 SizeMultiplier => new(0.66f, 1f);

	public override (Pin[] In, Pin[] Out) InitialPins => (
        new Pin[] {
            Pin.New<T>("X")
        },
        
        new Pin[] {
            Pin.New<T>("X²")
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

        if (typeof(T) == typeof(int)) {
            var x = GetInput<int>(0);
            SetOutput(0, x * x);
            return;
        }
        
        if (typeof(T) == typeof(long)) {
            var x = GetInput<long>(0);
            SetOutput(0, x * x);
            return;
        }

        throw new NotImplementedException();
    }
}

