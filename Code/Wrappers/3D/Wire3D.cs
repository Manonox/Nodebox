namespace Nodebox;

public sealed class Wire3D : Component
{
	[RequireComponent] public LineRenderer LineRenderer { get; set; }

	private Node3D _from;
	private int _fromIndex;
	private Node3D _to;
	private int _toIndex;

	[Property] public Node3D From { get => _from; set {
		if (value == _from) return;
		_from = value;
		UpdateWire();
	} }

	[Property] public int FromIndex { get => _fromIndex; set {
		if (value == _fromIndex) return;
		_fromIndex = value;
		UpdateWire();
	} }

	[Property] public Node3D To { get => _to; set {
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

	public static GameObject Connect(Node3D from, int fromIndex, Node3D to, int toIndex) {
		var go = new GameObject();
		var wire = go.AddComponent<Wire3D>();
		wire.From = from;
		wire.FromIndex = fromIndex;
		wire.To = to;
		wire.ToIndex = toIndex;
		return go;
	}

	public static GameObject Connect(GameObject from, int fromIndex, GameObject to, int toIndex) => Connect(from.GetComponent<Node3D>(), fromIndex, to.GetComponent<Node3D>(), toIndex);

	private void UpdateWire() {
		Wire?.Dispose();
		if (From == null) return;
		if (To == null) return;
		if (FromIndex < 0 | FromIndex >= From.Node.OutputPins.Count) return;
		if (ToIndex < 0 | ToIndex >= To.Node.InputPins.Count) return;
		Wire = new Wire(From.Node, FromIndex, To.Node, ToIndex);
	}

	protected override void OnStart() {
		LineRenderer.Color = Color.White;
		LineRenderer.Width = 0.5f;
		//LineRenderer.EndCap = SceneLineObject.CapStyle.Arrow;
		//LineRenderer.StartCap = SceneLineObject.CapStyle.Rounded;
		LineRenderer.Face = SceneLineObject.FaceMode.Camera;
		LineRenderer.CastShadows = false;
		LineRenderer.UseVectorPoints = true;
		LineRenderer.VectorPoints = new List<Vector3> {
			new(), new()
		};
	}

	protected override void OnPreRender() {
		LineRenderer.Enabled = false;
		if (Wire == null) return;
		var fromPos = From.GetWorldPinPosition(PinType.Output, FromIndex);
		var toPos = To.GetWorldPinPosition(PinType.Input, ToIndex);
		if (!fromPos.HasValue || !toPos.HasValue) return;
		
		LineRenderer.Enabled = true;
		LineRenderer.VectorPoints[0] = fromPos.Value;
		LineRenderer.VectorPoints[1] = toPos.Value;
	}

	protected override void OnDestroy() {
		Wire.Dispose();
	}
}
