namespace Nodebox.Nodes;

[RegisterNode]
[Generics(typeof(float))]
[Generics(typeof(double))]
public class Pi<T> : Node
{
	public override Type[] Generics => [typeof(T)];

	public override string Name => $"Pi<{typeof(T).GetPrettyName()}>";
    public override string Desc => null;
    public override string[] Groups => new string[] { "Math", "Constant" };
    public override string[] Aliases => [ "Pie" ];
	public override Vector2 SizeMultiplier => new(0.75f, 1f);

    public override (Pin[] In, Pin[] Out) InitialPins => (
        new Pin[] {
        },
        
        new Pin[] {
            Pin.New<T>(""),
        }
    );

    public Pi() {
        if (typeof(T) == typeof(float)) {
            SetOutput(0, MathF.PI);
            return;
        }
        else if (typeof(T) == typeof(double)) {
            SetOutput(0, Math.PI);
            return;
        }

        throw new Exception("wtf");
    }

	private DisplayPanel DisplayPanel { get; set; }
	public override void Render(GameObject go, Panel panel)
	{
		DisplayPanel = go.GetOrAddComponent<DisplayPanel>();
        DisplayPanel.Panel.Parent = panel;
        DisplayPanel.Value = "π";
        DisplayPanel.FontSizeOverride = 40f;
	}
}

