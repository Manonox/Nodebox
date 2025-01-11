namespace Nodebox.Nodes;

public class GameTime : Node
{
	public override bool Tick => true;
	public override string Name => "Game Time";
    public override string Desc => "Returns the time since the game started";
    public override string[] Groups => new string[] {"Time"};

    public GameTime()
    {
        OutputPins = new[]
        {
            new Pin<float>("Seconds")
        };
    }

    public override void Eval() {
        SetOutput<float>(0, Time.Now);
    }
}
