namespace Nodebox;

public class Node3D : PanelComponent, INodeWrapper
{
    [RequireComponent] public Sandbox.WorldPanel WorldPanel { get; set; } // Probably shouldn't be here.. ?
    [RequireComponent] public NodePanel NodePanel { get; set; }
	[RequireComponent] public LineRenderer LineRenderer { get; set; }

    private Node _node;
	public Node Node { get => _node; set {
		var old = _node;
		_node = value;
		UpdatePanel(old);
	} }

	protected void UpdatePanel(Node old) {
		if (old != null) {
			old.OnEvaluated -= OnNodeEvaluated;
		}

		if (NodePanel == null) return;
		if (Node == null) return;
		NodePanel.Name = Node.Name;
		NodePanel.InputPins = Node.InputPins;
		NodePanel.OutputPins = Node.OutputPins;
		NodePanel.Groups = Node.Groups;
		Node.OnEvaluated += OnNodeEvaluated;

		var maxPinsVertically = Math.Max(NodePanel.InputPins.Length, NodePanel.OutputPins.Length);
		WorldPanel.PanelSize = WorldPanel.PanelSize.WithY(128 + maxPinsVertically * 128);
		
		Node.Render(this.GameObject, NodePanel.CenterPanel);
	}

	protected override void OnAwake() {
		UpdatePanel(null);
	}

	protected override void OnFixedUpdate() {
		if (Node != null && Node.Tick)
			Node.Evaluate();
	}

	private void OnNodeEvaluated(Node node) {
		Node.Render(this.GameObject, NodePanel.CenterPanel);
	}
}

