namespace Nodebox.Nodes;

public sealed class GameTime : Node
{
	public override bool Tick => true;
	public override string Name => "Game Time";
    public override string Desc => "Returns the time since the game started";
    public override string[] Groups => new string[] {"Time"};

    public override (Pin[] In, Pin[] Out) InitialPins => (
        new Pin[] {
        },
        
        new Pin[] {
            new Pin<float>("Seconds")
        }
    );

    public override void Evaluate() {
        SetOutput(0, Time.Now);
    }
}
