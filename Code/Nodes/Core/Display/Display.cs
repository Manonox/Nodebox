namespace Nodebox.Nodes;


[Register]
[Description("Takes any input and displays it")]
[Tag("Core")]
[Reader]
public class Display : Node
{
	public override (Pin[] In, Pin[] Out) InitialPins => (
        new Pin[] {
            Pin.New<object>("*")
        },

        []
    );

	public DisplayPanel DisplayPanel { get; private set; }
	public override void Render(Panel panel)
	{
        if (DisplayPanel == null) {
            DisplayPanel = new DisplayPanel();
            Assert.NotNull(panel);
            DisplayPanel.Parent = panel;
            DisplayPanel.FitText = true;
        }

        DisplayPanel.Value = GetInput<object>(0);
        DisplayPanel.StateHasChanged();
	}
}
