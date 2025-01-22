namespace Nodebox.Nodes;

[Register]
[Description("Converts Hue, Saturation, Value into the corresponding RGB color")]
[Tag("Color")]
public class HsvToRgb : Node
{
    public override (Pin[] In, Pin[] Out) InitialPins => (
        new Pin[] {
            Pin.New<float>("Hue"),
            Pin.New<float>("Saturation"),
            Pin.New<float>("Value"),
            Pin.New<float>("Alpha"),
        },
        
        new Pin[] {
            Pin.New<Color>("RGB"),
            Pin.New<Color>("RGBA"),
        }
    );

    public HsvToRgb() {
        // SetInput(3, 1f);
        // SetOutput(0, new Color(1f, 1f, 1f));
        // SetOutput(1, new Color(1f, 1f, 1f, 1f));
    }

    public override void Evaluate() {
        SetOutput<Color>(0, new ColorHsv(GetInput<float>(0), GetInput<float>(1), GetInput<float>(2)));
        SetOutput<Color>(1, new ColorHsv(GetInput<float>(0), GetInput<float>(1), GetInput<float>(2), GetInput<float>(3)));
    }
}

