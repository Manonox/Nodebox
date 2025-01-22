namespace Nodebox.Nodes;


[Register(typeof(Library.All))]
[Description("Continuously reads a property")]
[Tag("Core")]
[Reader]
[Initialized]
public class Source<T> : Node
{
	public override (Pin[] In, Pin[] Out) InitialPins => (
        new Pin[] {
        },

        new Pin[] {
            Pin.New<T>("*"),
        }
    );

    public Source() {
        SetOutput<T>(0, default);
    }

    public Source(Reference reference) {
        Reference = reference;
        SetOutput(0, reference.Read<T>());
    }

    public Source(Reference<T> reference) {
        Reference = reference;
        SetOutput(0, reference.Read());
    }

    public Reference Reference { get; set; }

	public DisplayPanel DisplayPanel { get; set; }
	public override void Render(Panel panel) {
        if (DisplayPanel == null) {
		    DisplayPanel = new DisplayPanel();
            Assert.NotNull(panel);
	        DisplayPanel.Parent = panel;
            DisplayPanel.Style.FontSize = 12;
            DisplayPanel.Style.Height = Length.Percent(70f);
        }

	    DisplayPanel.Value = $"on {Reference?.Target ?? "null"}";
	}

	public override void Evaluate() {
        RenamePin(PinType.Output, 0, Reference?.PropertyName ?? "???");
        if (Reference == null) return;
		if (!Reference.TryRead(out var value)) return;
        SetOutput(0, value);
	}
}
