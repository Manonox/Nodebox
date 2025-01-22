namespace Nodebox.Nodes;

[Register]
[Tag("Logic")]
[Alias("!")]
public class Not : Node
{
    public override (Pin[] In, Pin[] Out) InitialPins => (
        new Pin[] {
            Pin.New<bool>("A"),
        },
        
        new Pin[] {
            Pin.New<bool>("!A")
        }
    );

	public override void Evaluate() => SetOutput(0, !GetInput<bool>(0));
}
