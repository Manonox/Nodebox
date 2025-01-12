namespace Nodebox.Nodes;

public sealed class FunnyRotation : Node
{
    public override string Name => "FunnyRotation";
    public override string Desc => "...";
    public override string[] Groups => new string[] { "Testing" };

    public override (Pin[] In, Pin[] Out) InitialPins => (
        new Pin[] {
            new Pin<float>("Yaw")
        },
        
        new Pin[] {
            new Pin<Rotation>("Rotation")
        }
    );

    public override void Evaluate() {
        var yaw = GetInput<float>(0);
        var rot = Rotation.FromYaw(yaw);
        SetOutput(0, rot);
    }
}

