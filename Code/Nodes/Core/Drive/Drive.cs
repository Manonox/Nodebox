namespace Nodebox.Nodes;


[RegisterNode]
[Hidden]
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

    public override Type[] Generics => [typeof(T)];

	public override bool Tick => true;
	public override string Name => $"Drive<{typeof(T).GetPrettyName()}>";
    public override string Desc => "Continuously writes to a property";
    public override string[] Groups => new string[] { "Core" };

	public override (Pin[] In, Pin[] Out) InitialPins => (
        new Pin[] {
            Pin.New<T>("*")
        },

        new Pin[] {
        }
    );

    public Reference Reference { get; set; }

	private DisplayPanel DisplayPanel { get; set; }
	public override void Render(GameObject go, Panel panel)
	{
		DisplayPanel = go.GetOrAddComponent<DisplayPanel>();
	    DisplayPanel.Panel.Parent = panel;
	    DisplayPanel.Value = $"on {Reference?.Target ?? "null"}";
        DisplayPanel.Panel.Style.FontSize = 12;
        DisplayPanel.Panel.Style.Height = Length.Percent(130f);
	}

	public override void Evaluate() {
        RenamePin(PinType.Input, 0, Reference?.PropertyName ?? "???");
        if (Reference == null) return;
		var value = GetInput<T>(0);
        Reference.Write(value);
	}
}
