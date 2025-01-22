namespace Nodebox.Nodes;


[Register]
[Tag("Math")]
[Polymorphic(true)]
public class Clamp : Node
{
    public override (Pin[] In, Pin[] Out) InitialPins => (
        new Pin[] {
            Pin.New<Polymorphic>("X"),
            Pin.New<Polymorphic>("Min"),
            Pin.New<Polymorphic>("Max"),
        },
        
        new Pin[] {
            Pin.New<Polymorphic>("Clamped X")
        }
    );

	public override Node Polymorph(PinWireChange change) =>
        PolymorphHelpers.ToConnectedTypeIfRegistered(typeof(Clamp<>), change);
}


[Register(typeof(Library.Integer))]
[Register(typeof(Library.Float))]
[Register(typeof(Library.Vector))]
[Register(typeof(Color))]
[Polymorphic(typeof(Clamp))]
public class Clamp<T> : Clamp
{
    public override (Pin[] In, Pin[] Out) InitialPins => base.InitialPins.SubstitutePolymorphic<T>();

    public override void Evaluate() {
        if (typeof(T) == typeof(float)) {
            SetOutput(0, GetInput<float>(0).Clamp(GetInput<float>(1), GetInput<float>(2)));
            return;
        }
        
        if (typeof(T) == typeof(double)) {
            SetOutput(0, GetInput<double>(0).Clamp(GetInput<double>(1), GetInput<double>(2)));
            return;
        }

        if (typeof(T) == typeof(Vector2)) {
            SetOutput(0, GetInput<Vector2>(0).Clamp(GetInput<Vector2>(1), GetInput<Vector2>(2)));
            return;
        }

        if (typeof(T) == typeof(Vector3)) {
            SetOutput(0, GetInput<Vector3>(0).Clamp(GetInput<Vector3>(1), GetInput<Vector3>(2)));
            return;
        }
        
        if (typeof(T) == typeof(Vector4)) {
            SetOutput(0, GetInput<Vector4>(0).Clamp(GetInput<Vector4>(1), GetInput<Vector4>(2)));
            return;
        }

        if (typeof(T) == typeof(Vector2Int)) {
            SetOutput(0, GetInput<Vector2Int>(0).Clamp(GetInput<Vector2Int>(1), GetInput<Vector2Int>(2)));
            return;
        }

        if (typeof(T) == typeof(Vector3Int)) {
            SetOutput(0, GetInput<Vector3Int>(0).Clamp(GetInput<Vector3Int>(1), GetInput<Vector3Int>(2)));
            return;
        }

        if (typeof(T) == typeof(Color)) {
            SetOutput(0, GetInput<Color>(0).Clamp(GetInput<Color>(1), GetInput<Color>(2)));
            return;
        }

        throw new NotImplementedException();
    }

	public override Node Polymorph(PinWireChange change) =>
        PolymorphHelpers.ToNonGenericIfAllDisconnected<Clamp>(change);
}
