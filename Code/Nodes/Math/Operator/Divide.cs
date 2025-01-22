namespace Nodebox.Nodes;

[Register]
[Tag("Math", "Operator")]
[Alias("/")]
[Polymorphic(true)]
public class Divide : Node
{
    public override (Pin[] In, Pin[] Out) InitialPins => (
        new Pin[] {
            Pin.New<Polymorphic>("A"),
            Pin.New<Polymorphic>("B"),
        },
        
        new Pin[] {
            Pin.New<Polymorphic>("A / B")
        }
    );

	public override Node Polymorph(PinWireChange change) =>
        PolymorphHelpers.ToConnectedTypeIfRegistered(typeof(Divide<>), change);
}


[Register(typeof(Library.FloatN))]
[Register(typeof(Library.IntegerN))]
[Register(typeof(Library.IntegerUnsigned))]
[Register(typeof(Angles))]
[Register(typeof(Color))]
[Polymorphic(typeof(Divide))]
public class Divide<T> : Divide
{
    public override (Pin[] In, Pin[] Out) InitialPins => base.InitialPins.SubstitutePolymorphic<T>();
    public override void Evaluate() {
        var t = typeof(T);
        
        if (t == typeof(float)) { SetOutput(0, GetInput<float>(0) / GetInput<float>(1)); return; }
        if (t == typeof(double)) { SetOutput(0, GetInput<double>(0) / GetInput<double>(1)); return; }
        if (t == typeof(Vector2)) { SetOutput(0, GetInput<Vector2>(0) / GetInput<Vector2>(1)); return; }
        if (t == typeof(Vector3)) { SetOutput(0, GetInput<Vector3>(0) / GetInput<Vector3>(1)); return; }
        if (t == typeof(Vector4)) { SetOutput(0, GetInput<Vector4>(0).Divide(GetInput<Vector4>(1))); return; }

        if (t == typeof(int)) { SetOutput(0, GetInput<int>(0) / GetInput<int>(1)); return; }
        if (t == typeof(long)) { SetOutput(0, GetInput<long>(0) / GetInput<long>(1)); return; }
        if (t == typeof(Vector2Int)) { SetOutput(0, GetInput<Vector2Int>(0) / GetInput<Vector2Int>(1)); return; }
        if (t == typeof(Vector3Int)) { SetOutput(0, GetInput<Vector3Int>(0) / GetInput<Vector3Int>(1)); return; }

        if (t == typeof(byte)) { SetOutput(0, GetInput<byte>(0) / GetInput<byte>(1)); return; }
        if (t == typeof(char)) { SetOutput(0, GetInput<char>(0) / GetInput<char>(1)); return; }

        if (t == typeof(Angles)) { SetOutput(0, GetInput<Angles>(0).Divide(GetInput<Angles>(1))); return; }
        if (t == typeof(Color)) { SetOutput(0, GetInput<Color>(0).Divide(GetInput<Color>(1))); return; }
    }

    public override Node Polymorph(PinWireChange change) =>
        PolymorphHelpers.ToNonGenericIfAllDisconnected<Divide>(change);

}
