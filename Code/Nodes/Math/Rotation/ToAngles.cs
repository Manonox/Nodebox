namespace Nodebox.Nodes;

[Register]
[Description("Converts Rotation into Angles (pitch, yaw, roll)")]
[Tag("Math", "Rotation", "Angles")]
[Alias("ToEuler")]
public class ToAngles : Node
{
    public override (Pin[] In, Pin[] Out) InitialPins => (
        new Pin[] {
            Pin.New<Rotation>("Rotation"),
        },
        
        new Pin[] {
            Pin.New<Angles>("Angles"),
        }
    );

    public override void Evaluate() {
        SetOutput(0, GetInput<Rotation>(0).Angles());
    }
}
