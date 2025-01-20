namespace Nodebox;

public class Wire3d : Component
{
	[RequireComponent] public LineRenderer LineRenderer { get; set; }

	public Vector3 Start { get => GameObject.WorldPosition; set => GameObject.WorldPosition = value; }
	public Vector3 End { get; set; } = Vector3.Zero;

	private Node3d _from;
	private int _fromIndex;
	private Node3d _to;
	private int _toIndex;

	[Property] public Node3d From { get => _from; set {
		if (value == _from) return;
		_from = value;
		UpdateWire();
	} }

	[Property] public int FromIndex { get => _fromIndex; set {
		if (value == _fromIndex) return;
		_fromIndex = value;
		UpdateWire();
	} }

	[Property] public Node3d To { get => _to; set {
		if (value == _to) return;
		_to = value;
		UpdateWire();
	} }

	[Property] public int ToIndex { get => _toIndex; set {
		if (value == _toIndex) return;
		_toIndex = value;
		UpdateWire();
	} }

	private Wire _wire;
	public Wire Wire { get => _wire; private set {
		var old = _wire;
		_wire = value;
		_wire.SetMeta(this);
		GameObject.Name = value.ToString();
	} }

	public static GameObject New(out Wire3d wire) {
		var go = new GameObject();
		wire = go.AddComponent<Wire3d>();
		return go;
	}

	public static GameObject Connect(Node3d from, int fromIndex, Node3d to, int toIndex) {
		var go = New(out var wire);
		wire.From = from;
		wire.FromIndex = fromIndex;
		wire.To = to;
		wire.ToIndex = toIndex;
		return go;
	}

	public static GameObject Connect(GameObject from, int fromIndex, GameObject to, int toIndex) => Connect(from.GetComponent<Node3d>(), fromIndex, to.GetComponent<Node3d>(), toIndex);

	private void UpdateWire() {
		Wire?.Dispose();
		if (!Enabled) return;
		//if (IsValidWire()) return;
		try {
			Wire = new Wire(From.Node, FromIndex, To.Node, ToIndex);
		} catch {
			Wire?.Dispose();
			// throw; // wtf
		}
	}

	
	[Pure]
	[System.Diagnostics.Contracts.Pure]
	public bool IsValidWire() {
		return GetWireException() == null;
	}

	[Pure]
	[System.Diagnostics.Contracts.Pure]
	public Exception GetWireException() {
		Wire testWire = null;
		try {
			testWire = new Wire(From.Node, FromIndex, To.Node, ToIndex);
		} catch (Exception e) {
			return e;
		}
		testWire?.Dispose();
		return null;
	}

	public Exception TrySet(Node3d node3d, PinType pinType, int? index = null) {
		var oldEnabled = Enabled;
		Enabled = false;

		var oldIndex = pinType == PinType.Output ? FromIndex : ToIndex;
		if (index != null) {
			if (pinType == PinType.Output) {
				FromIndex = index.Value;
			} else {
				ToIndex = index.Value;
			}
		}

		if (pinType == PinType.Output) {
			From = node3d;
		} else {
			To = node3d;
		}
		
		var exception = GetWireException();
		Enabled = oldEnabled;
		
		if (exception != null) {
			if (pinType == PinType.Output) {
				From = null;
				FromIndex = oldIndex;
			} else {
				To = null;
				ToIndex = oldIndex;
			}
			return exception;
		}

		return null;
	}

	public bool TrySetAny(Node3d node3d, PinType pinType) {
		var pins = node3d.Node.GetPins(pinType);
		foreach (var item in pins.Enumerate()) {
			if (TrySet(node3d, pinType, item.Index) == null) {
				return true;
			}
		}

		return false;
	}
	
	public Exception TrySetFrom(Node3d from, int? index = null) => TrySet(from, PinType.Output, index);
	public Exception TrySetTo(Node3d to, int? index = null) => TrySet(to, PinType.Input, index);

	protected override void OnStart() {
		UpdateWire();
		
		LineRenderer.Color = Color.White;
		LineRenderer.Width = 0.6f;
		LineRenderer.EndCap = SceneLineObject.CapStyle.Arrow;
		// LineRenderer.StartCap = SceneLineObject.CapStyle.Rounded;
		LineRenderer.Face = SceneLineObject.FaceMode.Camera;
		LineRenderer.CastShadows = false;
		LineRenderer.UseVectorPoints = true;
		LineRenderer.RenderOptions.Bloom = true;
		LineRenderer.Enabled = true;
		LineRenderer.VectorPoints = new List<Vector3> {
			new(), new(), new(), new(),
		};
	}

	protected override void OnDisabled() {
		Wire?.Dispose();
	}

	protected override void OnEnabled() {
		UpdateWire();
	}

	protected override void OnUpdate() {
		if (From != null) {
			var fromPos = From.GetWorldPinPosition(PinType.Output, FromIndex);
			Start = fromPos ?? Start;
		}
		
		if (To != null) {
			var toPos = To.GetWorldPinPosition(PinType.Input, ToIndex);
			End = toPos ?? End;
		}
		
		var startExtra = From?.WorldTransform.NormalToWorld(new Vector3(0f, 1f, 0f)) ?? Vector3.Zero;
		var endExtra = To?.WorldTransform.NormalToWorld(new Vector3(0f, -1f, 0f)) ?? Vector3.Zero;

		
		LineRenderer.VectorPoints[0] = Start + startExtra * 0.8f;
		LineRenderer.VectorPoints[1] = Start + startExtra * 2.8f;
		LineRenderer.VectorPoints[2] = End + endExtra * 5f;
		LineRenderer.VectorPoints[3] = End + endExtra * 3f;
		
		var colorStart = From?.Node.OutputPins[FromIndex].GetColor();
		var colorEnd = To?.Node.InputPins[ToIndex].GetColor();
		colorEnd ??= colorStart;
		colorStart ??= colorEnd;
		colorStart ??= Color.White;
		colorEnd ??= Color.White;

		LineRenderer.Color = new Gradient(
			new Gradient.ColorFrame(0.0f, SrgbToLinear(colorStart.Value)),
			new Gradient.ColorFrame(0.25f, SrgbToLinear(colorStart.Value)),
			new Gradient.ColorFrame(0.75f, SrgbToLinear(colorEnd.Value)),
			new Gradient.ColorFrame(1.0f, SrgbToLinear(colorEnd.Value))
		);
	}

	protected override void OnDestroy() {
		Wire?.Dispose();
	}

	private static Color SrgbToLinear(Color color) {
		static float srgbToLinear(float x) {
			if (x <= 0.04045f)
				return x / 12.92f;
			return MathF.Pow( (x + 0.055f) / 1.055f, 2.4f );
		}

		return new Color(
			srgbToLinear(color.r),
			srgbToLinear(color.g),
			srgbToLinear(color.b),
			color.a
		);
	}
}
