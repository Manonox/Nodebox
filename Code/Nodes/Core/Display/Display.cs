namespace Nodebox.Nodes;

public class Display : Node
{
    public override string Name => "Display";
    public override string Desc => "Takes any input and displays it";
    public override string[] Groups => new string[] {"Core"};

    public Display()
    {
        InputPins = new[]
        {
            new Pin<object>("*")
        };
    }

    private DisplayPanel DisplayPanel { get; set; }
	public override void Render(GameObject go, Panel panel)
	{
		DisplayPanel = go.GetOrAddComponent<DisplayPanel>();
        DisplayPanel.Panel.Parent = panel;
        DisplayPanel.Value = GetInput<object>(0);
	}
}
