namespace Nodebox.Nodes;


[Register(typeof(bool))]

[Register(typeof(float))]
[Register(typeof(double))]

//[Register(typeof(byte))]
[Register(typeof(int))]
[Register(typeof(long))]

// [Register(typeof(char))]
// [Register(typeof(string))]

[Register(typeof(Vector2))]
[Register(typeof(Vector3))]
[Register(typeof(Vector4))]
[Register(typeof(Vector2Int))]
[Register(typeof(Vector3Int))]

[Register(typeof(Angles))]
[Register(typeof(Rotation))]

[Register(typeof(Color))]
[Description("Outputs a value of your choosing")]
[Tag("Core")]
public class Constant<T> : Node
{
    public override Vector2 SizeMultiplier => typeof(T) switch {
        _ when typeof(T) == typeof(bool) => new(0.75f, 1f),
        _ when typeof(T) == typeof(float) => new(1f, 0.8f),
        _ when typeof(T) == typeof(double) => new(1f, 0.8f),
        _ when typeof(T) == typeof(int) => new(1f, 0.8f),
        _ when typeof(T) == typeof(long) => new(1f, 0.8f),
        _ when typeof(T).IsVectorType() => new(1f, 0.75f + 0.25f * typeof(T).GetVectorTypeDimensions()),
        _ => Vector2.One,
    };

	public override (Pin[] In, Pin[] Out) InitialPins => (
        new Pin[] {
        },

        new Pin[] {
            Pin.New<T>("")
        }
    );

    public Constant() {
        Value = default;
        if (typeof(T) == typeof(Color)) {
            Value = (T)(object)new Color(1f, 1f, 1f);
        }
    }

    public Constant(T value) {
        Value = value;
    }

    
	public ConstantPanel ConstantPanel { get; private set; }
	public override void Render(Panel panel)
	{
        if (ConstantPanel == null) {
		    ConstantPanel = new ConstantPanel();
            Assert.NotNull(panel);
	        ConstantPanel.Parent = panel;
            ConstantPanel.OnChanged += OnPanelChanged;
        }

        ConstantPanel.Type = typeof(T);
        ConstantPanel.Value = (object)Value;

        if (typeof(Reference).IsAssignableFrom(typeof(T)) && Value != null) {
            ConstantPanel.Reference = (Reference)(object)Value;
        }
	}

    private void OnPanelChanged(ConstantPanel _panel) => _ = typeof(T) switch {
        _ when typeof(T) == typeof(bool) => Value = (T)(object)ConstantPanel.Bool,
        _ when typeof(T) == typeof(float) => Value = (T)(object)ConstantPanel.Float,
        _ when typeof(T) == typeof(double) => Value = (T)(object)ConstantPanel.Double,
        _ when typeof(T) == typeof(int) => Value = (T)(object)ConstantPanel.Int,
        _ when typeof(T) == typeof(long) => Value = (T)(object)ConstantPanel.Long,
        _ when typeof(T) == typeof(Vector2) => Value = (T)(object)ConstantPanel.Vector2,
        _ when typeof(T) == typeof(Vector3) => Value = (T)(object)ConstantPanel.Vector3,
        _ when typeof(T) == typeof(Vector4) => Value = (T)(object)ConstantPanel.Vector4,
        _ when typeof(T) == typeof(Vector2Int) => Value = (T)(object)ConstantPanel.Vector2Int,
        _ when typeof(T) == typeof(Vector3Int) => Value = (T)(object)ConstantPanel.Vector3Int,
        _ when typeof(T) == typeof(Angles) => Value = (T)(object)ConstantPanel.Angles,
        _ when typeof(T) == typeof(Rotation) => Value = (T)(object)ConstantPanel.Rotation,
        _ when typeof(T) == typeof(Color) => Value = (T)(object)ConstantPanel.Color,
        _ => Value
    };

    public T _value;
    public T Value { get => _value; set {
        _value = value;
        MarkAsDirty();
    } }
    
	public override void Evaluate() => SetOutput(0, Value);
}
