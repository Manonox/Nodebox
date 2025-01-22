namespace Nodebox.Nodes;

[Register]
[Description("Returns the input value with it's sign flipped")]
[Tag("Math", "Operator")]
[Alias("-")]
[Polymorphic(true)]
public class Negate : Node
{
    public override (Pin[] In, Pin[] Out) InitialPins => (
        new Pin[] {
            Pin.New<Polymorphic>("A"),
        },
        
        new Pin[] {
            Pin.New<Polymorphic>("-A")
        }
    );

	public override Node Polymorph(PinWireChange change) =>
        PolymorphHelpers.ToConnectedTypeIfRegistered(typeof(Negate<>), change);
}

[Register(typeof(Library.FloatN))]
[Register(typeof(Library.IntegerN))]
[Register(typeof(Library.IntegerUnsigned))]
[Register(typeof(Angles))]
[Register(typeof(Color))]
[Polymorphic(typeof(Negate))]
public class Negate<T> : Negate
{
    public override (Pin[] In, Pin[] Out) InitialPins => base.InitialPins.SubstitutePolymorphic<T>();

    public override void Evaluate() {
        var t = typeof(T);
        
        if (t == typeof(float)) { SetOutput(0, -GetInput<float>(0)); return; }
        if (t == typeof(double)) { SetOutput(0, -GetInput<double>(0)); return; }
        if (t == typeof(Vector2)) { SetOutput(0, -GetInput<Vector2>(0)); return; }
        if (t == typeof(Vector3)) { SetOutput(0, -GetInput<Vector3>(0)); return; }
        if (t == typeof(Vector4)) { SetOutput(0, -GetInput<Vector4>(0)); return; }

        if (t == typeof(int)) { SetOutput(0, -GetInput<int>(0)); return; }
        if (t == typeof(long)) { SetOutput(0, -GetInput<long>(0)); return; }
        if (t == typeof(Vector2Int)) { SetOutput(0, GetInput<Vector2Int>(0).Negate()); return; }
        if (t == typeof(Vector3Int)) { SetOutput(0, GetInput<Vector3Int>(0).Negate()); return; }

        if (t == typeof(byte)) { SetOutput(0, -GetInput<byte>(0)); return; }
        if (t == typeof(char)) { SetOutput(0, -GetInput<char>(0)); return; }

        if (t == typeof(Angles)) { SetOutput(0, GetInput<Angles>(0).Negate()); return; }
        if (t == typeof(Color)) { SetOutput(0, GetInput<Color>(0).Negate()); return; }

        throw new NotImplementedException();
    }
}

