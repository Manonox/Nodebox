namespace Nodebox.Nodes;

[RegisterNode]
public class ToAngles : Node
{    
    public override string Name => "ToAngles";
    public override string Desc => "Converts Rotation into Angles";
    public override string[] Groups => [ "Math", "Angles", "Rotation" ];
    public override string[] Aliases => [ "ToEuler" ];

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
