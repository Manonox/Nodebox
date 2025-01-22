namespace Nodebox.Nodes;

[Register]
[Description("Converts Angles into Rotation")]
[Tag("Math", "Angles", "Rotation")]
[Alias("FromEuler", "FromAngles")]
public class ToRotation : Node
{
    public override (Pin[] In, Pin[] Out) InitialPins => (
        new Pin[] {
            Pin.New<Angles>("Angles"),
        },
        
        new Pin[] {
            Pin.New<Rotation>("Rotation"),
        }
    );

    public override void Evaluate() {
        SetOutput(0, GetInput<Angles>(0).ToRotation());
    }
}
