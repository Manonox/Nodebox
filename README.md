# Nodebox
**A UGC (User-Generated-Content) Node Graph Library for S&Box**
## Description
**Nodebox** is a reimagining of Wiremod gates from **Garry's Mod** made in **S&Box**

*⚠️ The project is in it's infancy, expect bugs and constant breaking changes ⚠️*

Inspired by **Unreal Engine's** [**Blueprints**](https://dev.epicgames.com/documentation/en-us/unreal-engine/blueprints-visual-scripting-in-unreal-engine) and [**Resonite**](https://store.steampowered.com/app/2519830/Resonite/)'s [**Protoflux**](https://wiki.resonite.com/ProtoFlux)

- The `Node`s themselves are separate from any visual/interactive parts
- There should be wrappers for them, which provide different visualization/interactions/functionality, etc.
    - Currently, only `Node3D` exists, which displays the inner `Node` via a `WorldPanel` (*1 `Node` = 1 `WorldPanel`*)
- Each `Node` type and their `Pin`s have some simple metadata attached to it (Name, Desc, etc.)

## Todo
- [x] `Wire3D` - `Wire` 3D wrapper (like Node3D)
- [x] Colors for `Wire3D`s (or `Wire` in general)
- [x] A `Node` type collection
  - [ ] Search functionality
- [x] A tool to drag around, copy, select and delete `Node3D`s, connect/disconnect pins
- [x] Read/Write for `Component` Property values
- [ ] A `Pin` type, that acts as a "pulse", which makes `Node`s work more like UE Blueprints (or Resonite's "Discrete" nodes)
- [ ] ~~Implement a `Node` variant code generator tool (for `Node`s like `Add<T>` (where T is float, double, VectorN, ...))~~
- [ ] Figure out networking
- [ ] Collapse/Uncollapse ([Pack/Unpack in Protoflux](https://wiki.resonite.com/ProtoFlux_Tool#Packing_ProtoFlux_nodes)) node graphs into GameObjects
- [ ] And, ofcourse, more `Node`s 🛠️

## Setup
**Nodebox** is an S&Box library *(for now..?)* , but it's not published yet.
### To install it (clone it):
1. Open the `<project>/Libraries/` folder (or create one if it's missing)
2. Open a cmd/shell and run `git clone git@github.com:Manonox/Nodebox.git`
3. Profit

## Usage
### Creating node graphs via a tool
Put the `Node3dTool` component on your first person camera
- Q(`menu`) opens the `Node` spawn menu
- R(`reload`) opens the context menu on GameObjects
  - (Property List / Destroy / Duplicate)
- Left Mouse Button(`attack1`)
  - Hold to drag `Node`s
  - Click pins to create `Wire`s
  - Double click while holding a `Wire` to spawn a `Constant<T>` or a `Display` node
- Right Mouse Button(`attack2`)
  - Drag left/right while holding a `Node` to rotate it on the Z axis(yaw)
  - Click pins to disconnect all `Wire`s (if holding a `Wire` cancels/destroys it)

### Generated node graph example:
**Place the GameTime->Square->Display combo with Node3D wrappers in the world**
```cs
protected override void OnStart()
{
    var nodes = new List<Node>() {
        new Nodebox.Nodes.GameTime(),
        new Nodebox.Nodes.Square<float>(),
        new Nodebox.Nodes.Display(),
    };

    var gos = nodes.Enumerate().Select((pair) => {
        var (index, x) = pair;
        var go = Node3D.Wrap(x);
        go.WorldPosition = new Vector3(80f, -index * 30f, 50f);
        go.WorldRotation = Rotation.FromYaw(180f);
        return go;
    }).ToList();

    Wire3D.Connect(gos[0], 0, gos[1], 0);
    Wire3D.Connect(gos[1], 0, gos[2], 0);
}
```
