namespace Nodebox.Nodes;


[Register(typeof(Library.All))]
[Description("Continuously writes to a property")]
[Tag("Core")]
[Writer]
[Initialized]
public class Drive<T> : Node
{
    public Drive() {
        Reference = null;
        SetInput<T>(0, default);
    }

    public Drive(Reference reference) {
        Reference = reference;
        SetInput(0, reference.Read<T>());
    }

    public Drive(Reference<T> reference) {
        Reference = reference;
        SetInput(0, reference.Read());
    }

	public override (Pin[] In, Pin[] Out) InitialPins => (
        new Pin[] {
            Pin.New<T>("*")
        },

        new Pin[] {
        }
    );

    public Reference Reference { get; set; }

	public DisplayPanel DisplayPanel { get; private set; }
	public override void Render(Panel panel)
	{
        if (DisplayPanel == null) {
		    DisplayPanel = new DisplayPanel();
            Assert.NotNull(panel);
	        DisplayPanel.Parent = panel;
            DisplayPanel.Style.FontSize = 12;
            DisplayPanel.Style.Height = Length.Percent(130f);
        }

	    DisplayPanel.Value = $"on {Reference?.Target ?? "null"}";
	}

	public override void Evaluate() {
        RenamePin(PinType.Input, 0, Reference?.PropertyName ?? "???");
        if (Reference == null) return;
		var value = GetInput<T>(0);
        Reference.Write(value);
	}
}
