namespace Nodebox.Nodes;


[RegisterNode]
[Generics(typeof(bool))]
[Generics(typeof(float))]
[Generics(typeof(double))]
[Generics(typeof(int))]
// [Generics(typeof(long))]

[Generics(typeof(Vector2))]
[Generics(typeof(Vector3))]
[Generics(typeof(Vector4))]
[Generics(typeof(Vector2Int))]
[Generics(typeof(Vector3Int))]

[Generics(typeof(Angles))]
[Generics(typeof(Rotation))]

[Generics(typeof(Color))]

public class Constant<T> : Node
{
    public override Type[] Generics => [typeof(T)];

	public override string Name => $"Constant<{typeof(T).GetPrettyName()}>";
    public override string Desc => "Outputs a value of your choosing";
    public override string[] Groups => new string[] { "Core" };
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

    
	private ConstantPanel ConstantPanel { get; set; }
	public override void Render(GameObject go, Panel panel)
	{
        if (ConstantPanel == null) {
		    ConstantPanel = go.GetOrAddComponent<ConstantPanel>();
            ConstantPanel.OnChanged += OnPanelChanged;
        }

	    ConstantPanel.Panel.Parent = panel;
        ConstantPanel.Type = typeof(T);

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
