namespace Nodebox.Nodes;


[RegisterNode]
[Hidden]
public class Source<T> : Node
{
    public override Type[] Generics => [typeof(T)];

    public override bool Tick => true;
    public override string Name => "Source";
    public override string Desc => "Continuously reads a property";
    public override string[] Groups => new string[] { "Core" };

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

	private DisplayPanel DisplayPanel { get; set; }
	public override void Render(GameObject go, Panel panel) {
		DisplayPanel = go.GetOrAddComponent<DisplayPanel>();
	    DisplayPanel.Panel.Parent = panel;
	    DisplayPanel.Value = $"on {Reference?.Target ?? "null"}";
        DisplayPanel.Panel.Style.FontSize = 12;
        DisplayPanel.Panel.Style.Height = Length.Percent(70f);
	}

	public override void Evaluate() {
        RenamePin(PinType.Output, 0, Reference?.PropertyName ?? "???");
        if (Reference == null) return;
		if (!Reference.TryRead(out var value)) return;
        SetOutput(0, value);
	}
}
