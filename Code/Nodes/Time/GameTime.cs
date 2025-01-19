namespace Nodebox.Nodes;


[RegisterNode]
public class GameTime : Node
{
	public override bool Tick => true;
	public override string Name => "Game Time";
    public override string Desc => "Returns the time since the game started";
    public override string[] Groups => new string[] {"Time"};
    public override Vector2 SizeMultiplier => new(0.66f, 1f);

    public override (Pin[] In, Pin[] Out) InitialPins => (
        new Pin[] {
        },
        
        new Pin[] {
            Pin.New<float>("Seconds")
        }
    );

    public override void Evaluate() {
        SetOutput(0, Time.Now);
    }
}
