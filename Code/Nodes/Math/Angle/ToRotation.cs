namespace Nodebox.Nodes;

[RegisterNode]
public class ToRotation : Node
{    
    public override string Name => "ToRotation";
    public override string Desc => "Converts Angles into Rotation";
    public override string[] Groups => [ "Math", "Angles", "Rotation" ];
    public override string[] Aliases => [ "FromEuler", "FromAngles" ];

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
