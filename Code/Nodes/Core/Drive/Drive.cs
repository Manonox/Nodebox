using Sandbox.Diagnostics;

namespace Nodebox.Nodes;

public sealed class Drive<T> : Node
{
    public override bool Tick => true;
    public override string Name => "Drive";
    public override string Desc => "Continuously writes to a property";
    public override string[] Groups => new string[] { "Core" };

	public override (Pin[] In, Pin[] Out) InitialPins => (
        new Pin[] {
            new Pin<T>("*")
        },

        new Pin[] {
        }
    );

    public object Target { get; set; }
    
    public PropertyDescription Property { get; set; }

	private DisplayPanel DisplayPanel { get; set; }
	public override void Render(GameObject go, Panel panel)
	{
		DisplayPanel = go.GetOrAddComponent<DisplayPanel>();
	    DisplayPanel.Panel.Parent = panel;
	    DisplayPanel.Value = $"on {Target}";
        DisplayPanel.Panel.Style.FontSize = 12;
        DisplayPanel.Panel.Style.Height = Length.Percent(130f);
	}

	public override void Evaluate() {
        RenameInputPin(0, Property.Name);
		var value = GetInput<T>(0);
        Assert.True(Target != null);
        Assert.True(Property != null);
        Assert.True(Property.CanWrite);
        Assert.True(Property.TypeDescription == TypeLibrary.GetType(Target.GetType()));
        TypeLibrary.SetProperty(Target, Property.Name, value);
	}
}
