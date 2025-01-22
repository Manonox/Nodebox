namespace Nodebox.Nodes;

[Register]
[Description("Converts RGB into the corresponding Hue, Saturation, Value")]
[Tag("Color")]
public class RgbToHsv : Node
{
    public override (Pin[] In, Pin[] Out) InitialPins => (
        new Pin[] {
            Pin.New<Color>("Color"),
        },
        
        new Pin[] {
            Pin.New<float>("Hue"),
            Pin.New<float>("Saturation"),
            Pin.New<float>("Value"),
            Pin.New<float>("Alpha"),
        }
    );

    public override void Evaluate() {
        var color = GetInput<Color>(0);
        var hsv = color.ToHsv();
        SetOutput(0, hsv.Hue);
        SetOutput(1, hsv.Saturation);
        SetOutput(2, hsv.Value);
        SetOutput(3, hsv.Alpha);
    }
}

