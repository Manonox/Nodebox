namespace Nodebox.Nodes;

public class Square : Node
{
    public override string Name => "Square";
    public override string Desc => "Takes a number X and returns it's square";
    public override string[] Groups => new string[] { "Math" };

    public Square()
    {
        InputPins = new[]
        {
            new Pin<float>("X")
        };

        OutputPins = new[]
        {
            new Pin<float>("X²")
        };
    }

    public override void Eval() {
        var n = GetInput<float>(0);
        SetOutput(0, n*n);
    }
}

