namespace Nodebox.Nodes;

[Register]
[Tag("Logic")]
[Alias("^")]
public class Xor : Node
{
    public override (Pin[] In, Pin[] Out) InitialPins => (
        new Pin[] {
            Pin.New<bool>("A"),
            Pin.New<bool>("B"),
        },
        
        new Pin[] {
            Pin.New<bool>("A ^ B")
        }
    );

	public override void Evaluate() => SetOutput(0, GetInput<bool>(0) ^ GetInput<bool>(1));
}
