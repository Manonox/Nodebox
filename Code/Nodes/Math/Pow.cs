namespace Nodebox.Nodes;


// Should be a Polymorphic Node type..? (Turns itself into a node that works with the type that was connected into it)
// public class Pow : Node
// {
// 	public override string Name => "Pow<T>";
//     public override string Desc => "Returns A to the power B";
//     public override string[] Groups => new string[] { "Math" };

//     public Pow()
//     {
//         InputPins = new[]
//         {
//             new Pin<object>("A"),
//             new Pin<object>("B"),
//         };

//         OutputPins = new[]
//         {
//             new Pin<object>("A^B")
//         };
//     }                                                                                                                                                                                
// }

public class PowFloat : Node
{
	public override string Name => "Pow<float>";
    public override string Desc => "Returns A to the power B";
    public override string[] Groups => new string[] { "Math" };

    public PowFloat()
    {
        InputPins = new[]
        {
            new Pin<float>("A"),
            new Pin<float>("B"),
        };

        OutputPins = new[]
        {
            new Pin<float>("A^B")
        };
    }

    public override void Eval() {
        SetOutput(0, MathF.Pow(GetInput<float>(0), GetInput<float>(1)));
    }                                                                                                                                                                                   
}
 
public class PowDouble : Node
{
	public override string Name => "Pow<double>";
    public override string Desc => "Returns A to the power B";
    public override string[] Groups => new string[] { "Math" };

    public PowDouble()
    {
        InputPins = new[]
        {
            new Pin<float>("A"),
            new Pin<float>("B"),
        };

        OutputPins = new[]
        {
            new Pin<float>("A^B")
        };
    }

    public override void Eval() {
        SetOutput(0, Math.Pow(GetInput<double>(0), GetInput<double>(1)));
    }                                                                                                                                                                                   
}
