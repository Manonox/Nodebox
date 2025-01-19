using Nodebox.Nodes;
using Sandbox.Diagnostics;

namespace Nodebox;


using T = Dummy;

public class Node3dTool : Component
{
    [RequireComponent] public Sandbox.WorldInput WorldInput { get; set; }
    [RequireComponent] public ScreenPanel ScreenPanel { get; set; }
    [RequireComponent] public Node3dSpawnMenu Node3dSpawnMenu { get; set; }
    [RequireComponent] public Node3dContextMenu Node3dContextMenu { get; set; }
    [RequireComponent] public Node3dPropertyMenu Node3dPropertyMenu { get; set; }
    [RequireComponent] public Library Library { get; set; }
    
    [Property]
    [InputAction]
    public string ContextMenuAction { get; set; } = "Reload";

    [Property] public float ContextMenuRange { get; set; } = 1000f;
    [Property] public float SpinSpeed { get; set; } = 5f;
    [Property] public float ScrollSpeed { get; set; } = 5f;

    public object ContextMenuTarget { get; set; }
    public GameObject PropertyMenuTarget { get; set; }
    public object PropertyMenuTargetComponent { get; set; }

    public Node3d HeldNode { get; private set; }
    public (Wire3d Wire3d, PinType TargetPinType, float Distance)? HeldWire { get; private set; }

    public List<Node3d> SelectedNode3ds { get; private set; }

    public Transform? previousTransform;

    private (bool Down, bool Pressed, bool Released) LeftAction => (
        Input.Down(WorldInput.LeftMouseAction),
        Input.Pressed(WorldInput.LeftMouseAction),
        Input.Released(WorldInput.LeftMouseAction)
    );

    private TimeSince doubleLeftClickTimer;

    private (bool Down, bool Pressed, bool Released) RightAction => (
        Input.Down(WorldInput.RightMouseAction),
        Input.Pressed(WorldInput.RightMouseAction),
        Input.Released(WorldInput.RightMouseAction)
    );

    private (bool Down, bool Pressed, bool Released) ContextAction => (
        Input.Down(ContextMenuAction),
        Input.Pressed(ContextMenuAction),
        Input.Released(ContextMenuAction)
    );

	protected override void OnStart()
	{
        Node3dSpawnMenu.Library = Library;
        Node3dSpawnMenu.Enabled = false;

        Node3dContextMenu.Enabled = false;

        Node3dPropertyMenu.Library = Library;
        Node3dPropertyMenu.Enabled = false;
	}

    public GameObject OnNodeSpawnRequested(Library.Entry entry, bool autoAttach = true) {
        var go = entry.CreateNode3d();
        go.WorldPosition = WorldPosition + WorldRotation.Forward * 50f;
        go.WorldRotation = Rotation.FromYaw(WorldRotation.Yaw() - 180f);
        if (autoAttach)
            AttachToHeldWire(go);
        return go;
    }

    public void AttachToHeldWire(GameObject go) {
        if (HeldWire != null) {
            var (wire3d, targetPinType, _) = HeldWire.Value;
            var node3d = go.GetComponent<Node3d>();

            if (wire3d.TrySetAny(node3d, targetPinType)) {
                HeldWire = null;
                CloseSpawnMenu();
            }
        }
    }

	protected override void OnUpdate()
    {
        HandleActions();

        MoveHeldNode();
        MoveHeldWire();
        previousTransform = GameObject.WorldTransform;

        PaintHud();
    }
    
    private void HandleActions() {
        if (Input.Pressed("menu")) {
            if (!Node3dSpawnMenu.Panel.IsValid()) {
                CloseContextMenu();
                OpenSpawnMenu();
            } else {
                CloseSpawnMenu();
            }
        }

        if (LeftAction.Pressed) {
            var target = WorldInput.Hovered;
            if (CheckNode3d(target, out var node3d)) {
                if (target.GetType() != typeof(PinButton)) {
                    StartHoldingNode(node3d);
                } else {
                    OnPinLeftClicked(node3d, (PinButton)target);
                }
            }
            else if (HeldWire != null) {
                if (doubleLeftClickTimer.Relative < 0.2f) {
                    TrySpawnConstantOrDisplay();
                } else {
                    doubleLeftClickTimer = 0;
                }
            }
        }

        if (LeftAction.Released) {
            StopHoldingNode();
        }

        if (RightAction.Pressed) {
            if (HeldWire.HasValue) {
                DestroyHeldWire();
            } else {
                var target = WorldInput.Hovered;
                if (!CheckNode3d(target, out var node3d)) return;
                if (target.GetType() == typeof(PinButton)) {
                    OnPinRightClicked(node3d, (PinButton)target);
                }
            }
        }

        if (ContextAction.Pressed) {
            StopHoldingNode();
            if (Node3dContextMenu.Panel != null && Node3dContextMenu.Panel.IsValid()) {
                CloseContextMenu();
                return;
            }
            
            var target = WorldInput.Hovered;
            if (target == null) {
                var trace = Scene.Trace.Ray(new Ray(WorldPosition, WorldTransform.Forward), ContextMenuRange)
                    .WithoutTags("player")
                    .Run();
                var go = trace.Component?.GameObject;
                if (go != null) {
                    OpenContextMenu(go);
                }
            } else {
                if (!CheckNode3d(target, out var node3d)) return;
                OpenContextMenu(node3d.GameObject);
            }
        }
    }

    public void StartHoldingNode(Node3d node3d) {
        if (HeldNode != null) {
            StopHoldingNode();
        }

        HeldNode = node3d;
    }

    public void StopHoldingNode() {
        if (HeldNode == null) return;
        HeldNode = null;
    }

    private void MoveHeldNode() {
        if (HeldNode == null) return;
        if (!previousTransform.HasValue) return;
        var previous = previousTransform.Value;
        var current = GameObject.WorldTransform;

        var previousOffset = previous.PointToLocal(HeldNode.WorldPosition);
        previousOffset = previousOffset.WithX(previousOffset.x + Input.MouseWheel.y * ScrollSpeed);
        var offset = current.PointToWorld(previousOffset);
        HeldNode.WorldPosition = offset;
        HeldNode.WorldRotation *= Rotation.FromYaw(current.Rotation.Yaw() - previous.Rotation.Yaw()) * (RightAction.Down ? -SpinSpeed : 1f);
    }

    private void OnPinLeftClicked(Node3d node3d, PinButton pinButton) {
        var index = pinButton.Index;
        var pinType = pinButton.IsOutput ? PinType.Output : PinType.Input;
        if (HeldWire.HasValue) {
            var (heldWire, targetPinType, _) = HeldWire.Value;
            if (pinType != targetPinType) return;

            if (pinButton.IsOutput) {
                if (node3d == heldWire.To) return;
                
                var e = heldWire.TrySetFrom(node3d, index);
                if (e != null) {
                    Log.Warning(e);
                    return;
                }
            } else {
                if (node3d == heldWire.From) return;
                
                // Disconnect existing input pins
                var pinInputWires = node3d.Node.GetPinWires(PinType.Input, index).ToList();
                if (pinInputWires.Count > 0) {
                    pinInputWires[0].GetMeta<Wire3d>().To = null;
                }

                var e = heldWire.TrySetTo(node3d, index);
                if (e == null) {

                    // Delete old disconnected wires
                    if (pinInputWires.Count > 0) {
                        pinInputWires[0].GetMeta<Wire3d>().GameObject.DestroyImmediate();
                    }
                } else {
                    // Reinstantiate old connections
                    if (pinInputWires.Count > 0) {
                        pinInputWires[0].GetMeta<Wire3d>().To = node3d;
                    }

                    Log.Warning(e);
                    return;
                }
            }

            HeldWire = null;
            return;
        }

        var distance = node3d.GameObject.WorldPosition.Distance(GameObject.WorldPosition) - 10f;
        Wire3d wire;

        var inputWires = node3d.Node.GetPinWires(PinType.Input, index);
        if (pinType == PinType.Input && inputWires.Count() == 1) {
            wire = inputWires.First().GetMeta<Wire3d>();
            Assert.NotNull(wire, "wtf");
            wire.To = null;
            HeldWire = (wire, PinType.Input, distance);
            return;
        } else {
            Wire3d.New(out wire);
        }
        
        if (pinType == PinType.Output) {
            wire.From = node3d;
            wire.FromIndex = index;
        } else {
            wire.To = node3d;
            wire.ToIndex = index;
        }

        HeldWire = (wire, pinType.Opposite(), distance);
    }

    private void OnPinRightClicked(Node3d node3d, PinButton pinButton) {
        node3d.Node.GetPinWires(pinButton.IsOutput ? PinType.Output : PinType.Input, pinButton.Index).ForEach(x => {
            x.GetMeta<Wire3d>().GameObject.Destroy();
        });
    }

    private void DestroyHeldWire() {
        HeldWire.Value.Wire3d.GameObject.Destroy();
        HeldWire = null;
    }

    private void MoveHeldWire() {
        if (!HeldWire.HasValue) return;
        var (wire, targetPinType, distance) = HeldWire.Value;
        var position = GameObject.WorldTransform.PointToWorld(new Vector3(distance, 0f, 0f));
        if (targetPinType == PinType.Input) {
            wire.End = position;
        } else {
            wire.Start = position;
        }
    }

    public void OpenSpawnMenu() {
        CloseAllMenus();
        Node3dSpawnMenu.Enabled = false;
        Node3dSpawnMenu.Enabled = true;
        Node3dSpawnMenu.Panel.UserData = this;
    }

    public void CloseSpawnMenu() {
        Node3dSpawnMenu.Panel?.Delete();
    }

    public void OpenContextMenu(object obj) {
        CloseAllMenus();
        ContextMenuTarget = obj;
        
        if (ContextMenuTarget.GetType() == typeof(GameObject)) {
            var targetGameObject = (GameObject)ContextMenuTarget;
            Node3dContextMenu.Entries = new() {
                new("Destroy", "delete", () => {
                    targetGameObject.Destroy();
                    CloseContextMenu();
                }),
                new("Duplicate", "content_copy", () => {
                    var go = targetGameObject.Clone(targetGameObject.WorldTransform.WithPosition(targetGameObject.WorldPosition + Vector3.Up * 20f));
                    var dstNode3d = go.GetComponent<Node3d>();
                    var srcNode3d = targetGameObject.GetComponent<Node3d>();
                    if (srcNode3d != null) {
                        dstNode3d.Node = srcNode3d.Node.Clone();
                    }

                    CloseContextMenu();
                }),
                new("Property List", "list_alt", () => {
                    CloseContextMenu();
                    
                    OpenPropertyMenu(targetGameObject);
                })
            };
        }
        else if (ContextMenuTarget.GetType() == typeof(PropertyDescription)) {
            var gameObject = PropertyMenuTarget;
            var propertyDescription = (PropertyDescription)ContextMenuTarget;
            //var reference = new Reference(PropertyMenuTargetComponent, propertyDescription);
            var reference = TypeLibrary.GetType<Reference<T>>().CreateGeneric<Reference>(
                [propertyDescription.PropertyType],
                [PropertyMenuTargetComponent, propertyDescription]
                );

            Node3dContextMenu.Entries = new() {
                new("Source", "output", () => {
                    var node3dGo = OnNodeSpawnRequested(new Library.Entry(typeof(Source<T>), [propertyDescription.PropertyType]), false);
                    var node3d = node3dGo.GetComponent<Node3d>();
                    
                    var source = TypeLibrary.GetType<Source<T>>().CreateGeneric<Node>([propertyDescription.PropertyType], [reference]);
                    node3d.Node = source;
                    AttachToHeldWire(node3dGo);
                    CloseContextMenu();
                }),
                new("Drive", "input", () => {
                    var node3dGo = OnNodeSpawnRequested(new Library.Entry(typeof(Drive<T>), [propertyDescription.PropertyType]), false);
                    var node3d = node3dGo.GetComponent<Node3d>();
                    
                    var drive = TypeLibrary.GetType<Drive<T>>().CreateGeneric<Node>([propertyDescription.PropertyType], [reference]);
                    node3d.Node = drive;
                    AttachToHeldWire(node3dGo);
                    CloseContextMenu();
                }),
                new("Reference", "link", () => {
                    var node3dGo = OnNodeSpawnRequested(new Library.Entry(typeof(Constant<T>), [propertyDescription.PropertyType]), false);
                    var node3d = node3dGo.GetComponent<Node3d>();
                    
                    var referenceType = TypeLibrary.GetType<Reference<T>>().MakeGenericType([propertyDescription.PropertyType]);
                    node3d.Node = TypeLibrary.GetType<Constant<T>>().CreateGeneric<Node>([referenceType], [reference]);
                    AttachToHeldWire(node3dGo);
                    CloseContextMenu();
                }),
            };
        }
        else {
            return;
        }

        Node3dContextMenu.Enabled = false;
        Node3dContextMenu.Enabled = true;
        Node3dContextMenu.Panel.UserData = this;
    }

    public void CloseContextMenu() {
        Node3dContextMenu.Panel?.Delete();
    }

    public void OpenPropertyMenu(GameObject go) {
        CloseAllMenus();
        PropertyMenuTarget = go;
        Node3dPropertyMenu.TargetGameObject = go;
        Node3dPropertyMenu.Enabled = false;
        Node3dPropertyMenu.Enabled = true;
        Node3dPropertyMenu.Panel.UserData = this;
    }

    public void ClosePropertyMenu() {
        Node3dPropertyMenu.Panel?.Delete();
    }

    public void CloseAllMenus() {
        CloseSpawnMenu();
        CloseContextMenu();
        ClosePropertyMenu();
    }

    public void TrySpawnConstantOrDisplay() {
        var pinType = HeldWire.Value.TargetPinType;
        if (pinType == PinType.Input) {
            OnNodeSpawnRequested(new Library.Entry(typeof(Display), []));
        } else {
            var wire3d = HeldWire.Value.Wire3d;
            var type = wire3d.To.Node.InputPins[wire3d.ToIndex].Type;
            var constantNodeGenericType = TypeLibrary.GetType<Constant<T>>().TargetType;
            if (!Library.Entries.Where(x => x.Type == constantNodeGenericType && x.Generics.Count == 1 && x.Generics[0] == type).Any())
                return;

            OnNodeSpawnRequested(new Library.Entry(typeof(Constant<T>), [type]));
        }
    }

    private void PaintHud() {
        if ( Scene.Camera is null )
			return;

		var hud = Scene.Camera.Hud;
        var target = WorldInput.Hovered;
        if (target == null || !target.IsValid()) return;
        if (!CheckNode3d(target, out var node3d)) return;
        if (target.GetType() != typeof(PinButton)) return;
        var pinButton = (PinButton)target;
        var pinType = pinButton.IsOutput ? PinType.Output : PinType.Input;
        var index = pinButton.Index;
        var value = node3d.Node.GetPinValue(pinType, index);
        var pin = node3d.Node.GetPin(pinType, index);
        var text = $"{value?.ToString() ?? "null"} ({pin.Type.GetPrettyName()})";
        hud.DrawText(new TextRendering.Scope(text, Color.White.WithAlphaMultiplied(0.8f), 16, "Poppins", 600), Screen.Size * 0.51f, TextFlag.LeftTop);
    }

    private static bool CheckNode3d(Panel target, out Node3d node3d) {
        node3d = null;
        if (target == null) return false;
        var root = target.FindRootPanel();
        if (root == null) return false;
        if (root.GetType() != typeof(Sandbox.UI.WorldPanel)) return false;
        var worldPanel = (Sandbox.UI.WorldPanel)root;
        var userData = worldPanel.UserData;
        if (userData.GetType() != typeof(Node3d)) return false;
        node3d = (Node3d)userData;
        return true;
    }
}