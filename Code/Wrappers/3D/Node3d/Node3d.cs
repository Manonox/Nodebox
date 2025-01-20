using Sandbox.Diagnostics;

namespace Nodebox;

public class Node3d : PanelComponent, INodeWrapper<GameObject> {
    [RequireComponent] public Sandbox.WorldPanel WorldPanel { get; set; }
    [RequireComponent] public Node3dPanel Node3dPanel { get; set; }

    private Node _node;
	public Node Node { get => _node; set {
		var old = _node;
		_node = value;
		_node.SetMeta(this);
		GameObject.Name = value.ToString();
		UpdatePanel(old);
	} }

	public static GameObject Wrap(Node node) {
		var go = new GameObject();
		var node3d = go.AddComponent<Node3d>();
		node3d.Node = node;
		return go;
	}

	public GameObject Clone() => Wrap(_node.Clone());

	private void UpdatePanel(Node old) {
		if (Node3dPanel == null) return;
		if (Node == null) return;
		Node3dPanel.NodeRef = new WeakReference<Node>(Node);

		var maxPinsVertically = Math.Max(Node.InputPins.Count, Node.OutputPins.Count);
		var height = 48f + 42f + 16f * 2; // Header + Footer + Margins
		height += 72 * maxPinsVertically + 16f;
		WorldPanel.PanelSize = new Vector2(384f, height) * Node.SizeMultiplier;
		// WorldPanel.HorizontalAlign = Sandbox.WorldPanel.HAlignment.Left;
		// WorldPanel.VerticalAlign = Sandbox.WorldPanel.VAlignment.Top;
		WorldPanel.RenderOptions.Bloom = true;
		WorldPanel.GetPanel().UserData = this;
		
		//Node.Render(GameObject, NodePanel.CenterPanel); ?
	}

	public Vector3? GetLocalPinPosition(PinType type, int pinIndex) {
		Panel pinPanels = type == PinType.Input ? Node3dPanel.InputPinPanels : Node3dPanel.OutputPinPanels;
		if (pinPanels == null) return null;
		var dotPanel = pinPanels.GetChild(pinIndex).GetChild(type == PinType.Input ? 0 : 1).GetChild(0);
		var xy = dotPanel.Box.Rect.Center;
		xy *= WorldPanel.RenderScale * Sandbox.UI.WorldPanel.ScreenToWorldScale;
		return new Vector3(0f, xy.x, -xy.y);
	}

	public Vector3? GetWorldPinPosition(PinType type, int pinIndex) {
		var localPosition = GetLocalPinPosition(type, pinIndex);
		return localPosition.HasValue ? WorldTransform.PointToWorld(localPosition.Value) : null;
	}

	protected override void OnTreeBuilt() {
		if (Node == null) return;
		Node.Render(GameObject, Node3dPanel.CenterPanel);
	}

	protected override void OnAwake() {
		UpdatePanel(null);
	}

	protected override void OnDestroy() {
		if (Node == null) return;
		Node.GetAllWires().ForEach(x => x.GetMeta<Wire3d>().GameObject.DestroyImmediate());
		//Node.Dispose(); ..?
	}
}
