namespace Nodebox.Nodes;

public sealed class Display : Node
{
    public override string Name => "Display";
    public override string Desc => "Takes any input and displays it";
    public override string[] Groups => new string[] {"Core"};

	public override (Pin[] In, Pin[] Out) InitialPins => (
        new Pin[] {
            new Pin<object>("*")
        },

        new Pin[] {
        }
    );

	private DisplayPanel DisplayPanel { get; set; }
	public override void Render(GameObject go, Panel panel)
	{
		DisplayPanel = go.GetOrAddComponent<DisplayPanel>();
        DisplayPanel.Panel.Parent = panel;
        DisplayPanel.Value = GetInput<object>(0);
	}
}
