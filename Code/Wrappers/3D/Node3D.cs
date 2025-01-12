using Sandbox.Diagnostics;

namespace Nodebox;

public enum PinType {
	Input,
	Output,
}

public sealed class Node3D : PanelComponent, INodeWrapper<GameObject>
{
    [RequireComponent] public Sandbox.WorldPanel WorldPanel { get; set; } // Probably shouldn't be here.. ?
    [RequireComponent] public NodePanel NodePanel { get; set; }

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
		var node3d = go.AddComponent<Node3D>();
		node3d.Node = node;
		return go;
	}

	private void UpdatePanel(Node old) {
		if (NodePanel == null) return;
		if (Node == null) return;
		NodePanel.Name = Node.Name;
		NodePanel.InputPins = Node.InputPins;
		NodePanel.OutputPins = Node.OutputPins;
		NodePanel.Groups = Node.Groups;

		var maxPinsVertically = Math.Max(NodePanel.InputPins.Count, NodePanel.OutputPins.Count);
		WorldPanel.PanelSize = WorldPanel.PanelSize.WithY(128 + maxPinsVertically * 128);
		
		//Node.Render(GameObject, NodePanel.CenterPanel); ?
	}

	public Vector3? GetLocalPinPosition(PinType type, int pinIndex) {
		Panel pinPanels = type == PinType.Input ? NodePanel.InputPinPanels : NodePanel.OutputPinPanels;
		if (pinPanels == null) return null;
		var dotPanel = pinPanels.GetChild(pinIndex).GetChild(type == PinType.Input ? 0 : 1).GetChild(0);
		var xy = dotPanel.Box.Rect.Center;
		xy *= WorldPanel.RenderScale / 20f;
		return new Vector3(1f, xy.x, -xy.y);
	}

	public Vector3? GetWorldPinPosition(PinType type, int pinIndex) {
		var localPosition = GetLocalPinPosition(type, pinIndex);
		return localPosition.HasValue ? WorldTransform.PointToWorld(localPosition.Value) : null;
	}

	protected override void OnTreeBuilt()
	{
		Node.Render(GameObject, NodePanel.CenterPanel);
	}

	protected override void OnAwake() {
		UpdatePanel(null);
	}

	protected override void OnDestroy() {
		//Node.Dispose(); ..?
	}
}

public sealed class Node3DSystem : GameObjectSystem
{
	public Node3DSystem( Scene scene ) : base( scene )
	{
		Listen( Stage.PhysicsStep, 0, Tick, "Tick" );
	}

	void Tick()
	{
        var node3Ds = Scene.GetAllComponents<Node3D>();
		HashSet<Node3D> evaluated = new();
		List<Node3D> toEvaluate = node3Ds.Where((x) => x.Node.Tick).ToList();
		while (toEvaluate.Count > 0) {
			var toEvaluateNext = new List<Node3D>();
			foreach (var node3d in toEvaluate) {
				node3d.Node.Evaluate();
				node3d.Node.Render(node3d.GameObject, node3d.NodePanel.CenterPanel);
				evaluated.Add(node3d);

				var validOutputConnections = node3d.Node.ValidOutputConnections;
				validOutputConnections.ForEach(x => {
					x.From.TryGetTarget(out var from);
					x.To.TryGetTarget(out var to);
					Assert.True(from == node3d.Node);
					to._inputValues[x.ToIndex] = from._outputValues[x.FromIndex];
				});
				
				toEvaluateNext.AddRange(
					validOutputConnections
						.Select(x => {
							Assert.True(x.To.TryGetTarget(out var to), "wtf");
							return to.GetMeta<Node3D>();
						})
						.Where(x => !evaluated.TryGetValue(x, out _))
				);
			}

			toEvaluate = toEvaluateNext;
		}
	}
}
