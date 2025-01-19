namespace Nodebox.Nodes;

[RegisterNode]
[Generics(typeof(float))]
[Generics(typeof(double))]
public class E<T> : Node
{
	public override Type[] Generics => [typeof(T)];

	public override string Name => $"E<{typeof(T).GetPrettyName()}>";
    public override string Desc => null;
    public override string[] Groups => new string[] { "Math", "Constant" };

    public override (Pin[] In, Pin[] Out) InitialPins => (
        new Pin[] {
        },
        
        new Pin[] {
            Pin.New<T>(""),
        }
    );

    public E() {
        if (typeof(T) == typeof(float)) {
            SetOutput(0, MathF.E);
            return;
        }
        else if (typeof(T) == typeof(double)) {
            SetOutput(0, Math.E);
            return;
        }

        throw new Exception("wtf");
    }

	private DisplayPanel DisplayPanel { get; set; }
	public override void Render(GameObject go, Panel panel)
	{
		DisplayPanel = go.GetOrAddComponent<DisplayPanel>();
        DisplayPanel.Panel.Parent = panel;
        DisplayPanel.Value = "e";
        DisplayPanel.FontSizeOverride = 40f;
	}
}

