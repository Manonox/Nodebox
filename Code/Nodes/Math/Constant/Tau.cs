namespace Nodebox.Nodes;

[Register(typeof(Library.Float))]
[Description("The ratio of a circle's circumference to its radius")]
[Tag("Math", "Constant")]
public class Tau<T> : Node
{
	public override Vector2 SizeMultiplier => new(0.75f, 1f);

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

	public DisplayPanel DisplayPanel { get; private set; }
	public override void Render(Panel panel)
	{
        DisplayPanel ??= new DisplayPanel {
            Parent = panel,
            FontSizeOverride = 40f,
        };

        DisplayPanel.Value = "τ";
	}
}
