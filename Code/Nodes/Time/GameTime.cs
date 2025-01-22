namespace Nodebox.Nodes;


[Register]
[Description("Returns the time since the game started")]
[Tag("Time")]
[Alias("CurTime", "Now")]
[Reader]
public class GameTime : Node
{
    public override (Pin[] In, Pin[] Out) InitialPins => (
		Array.Empty<Pin>(),
        
        new Pin[] {
            Pin.New<float>("Seconds")
        }
    );

    public override void Evaluate() {
        SetOutput(0, Time.Now);
    }
}
