namespace Nodebox.Nodes;

[RegisterNode]
[Generics(typeof(float))]
[Generics(typeof(double))]
public class Tau<T> : Node
{
	public override Type[] Generics => [typeof(T)];

	public override string Name => $"Tau<{typeof(T).GetPrettyName()}>";
    public override string Desc => null;
    public override string[] Groups => new string[] { "Math", "Constant" };

    public override (Pin[] In, Pin[] Out) InitialPins => (
        new Pin[] {
        },
        
        new Pin[] {
            Pin.New<T>(""),
        }
    );

    public Tau() {
        if (typeof(T) == typeof(float)) {
            SetOutput(0, MathF.Tau);
            return;
        }
        else if (typeof(T) == typeof(double)) {
            SetOutput(0, Math.Tau);
            return;
        }

        throw new Exception("wtf");
    }

	private DisplayPanel DisplayPanel { get; set; }
	public override void Render(GameObject go, Panel panel)
	{
		DisplayPanel = go.GetOrAddComponent<DisplayPanel>();
        DisplayPanel.Panel.Parent = panel;
        DisplayPanel.Value = "τ";
        DisplayPanel.FontSizeOverride = 40f;
	}
}

