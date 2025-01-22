namespace Nodebox.Nodes;

[Register(typeof(Library.Float))]
[Description("Euler's number")]
[Tag("Math", "Constant")]
public class E<T> : Node
{
	public override Vector2 SizeMultiplier => new(0.75f, 1f);

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

	public DisplayPanel DisplayPanel { get; private set; }
	public override void Render(Panel panel)
	{
        DisplayPanel ??= new DisplayPanel {
            Parent = panel,
            FontSizeOverride = 40f,
        };

        DisplayPanel.Value = "e";
	}
}

