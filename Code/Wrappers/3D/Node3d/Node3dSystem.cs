namespace Nodebox;

public class Node3dSystem : GameObjectSystem
{
	public Node3dSystem( Scene scene ) : base( scene )
	{
		Listen( Stage.PhysicsStep, 10, Tick, "Tick" );
	}

	void Tick()
	{
        var node3Ds = Scene.GetAllComponents<Node3d>();
		HashSet<Node3d> evaluated = new();
		List<Node3d> toEvaluate = [.. node3Ds
			.Where(x => x.Node != null)
			.Where(x => {
				if (x.Node.Dirty) return true;
				var type = x.Node.GetType();
				return TypeLibrary.GetType(type).HasAttribute<ReaderAttribute>();
			})];
		while (toEvaluate.Count > 0) {
			var toEvaluateNext = new List<Node3d>();
			foreach (var node3d in toEvaluate) {
				Exception exception = null;
				try {
					node3d.Node.Evaluate();
				}
				catch (Exception e) {
					exception = e;
				}
				
				node3d.Node.Dirty = false;
				if (exception != null) {
                    Log.Error(exception);
                    continue;
                }

				if (node3d.Node3dPanel.CenterPanel != null) {
					node3d.Node.Render(node3d.Node3dPanel.CenterPanel);
				}

				evaluated.Add(node3d);

				var outputWires = node3d.Node.GetOutputWires();
				outputWires.ForEach(x => {
					Assert.True(x.From == node3d.Node);
					x.Pass();
				});
				
				toEvaluateNext.AddRange(
					outputWires
						.Where(x => x.To != null)
						.Select(x => x.To.GetMeta<Node3d>())
						.Where(x => !evaluated.TryGetValue(x, out _))
				);
			}

			toEvaluate = toEvaluateNext;
		}
	}
}
