namespace Nodebox.Nodes;

[Register]
[Tag("Operator")]
[Alias(">=")]
[Polymorphic(true)]
public class GreaterThanOrEqual : Node
{
    public override (Pin[] In, Pin[] Out) InitialPins => (
        new Pin[] {
            Pin.New<Polymorphic>("A"),
            Pin.New<Polymorphic>("B"),
        },
        
        new Pin[] {
            Pin.New<bool>("A >= B")
        }
    );

	public override Node Polymorph(PinWireChange change) =>
        PolymorphHelpers.ToConnectedInputTypeIfRegistered(typeof(GreaterThanOrEqual<>), change);
}


[Register(typeof(Library.Float))]
[Register(typeof(Library.Integer))]
[Register(typeof(Library.IntegerUnsigned))]
[Polymorphic(typeof(GreaterThanOrEqual))]
public class GreaterThanOrEqual<T> : GreaterThanOrEqual
{
    public override (Pin[] In, Pin[] Out) InitialPins => base.InitialPins.SubstitutePolymorphic<T>();
    public override void Evaluate() {
        var t = typeof(T);
        if (t == typeof(float)) { SetOutput(0, GetInput<float>(0) >= GetInput<float>(1)); return; }
        if (t == typeof(double)) { SetOutput(0, GetInput<double>(0) >= GetInput<double>(1)); return; }

        if (t == typeof(int)) { SetOutput(0, GetInput<int>(0) >= GetInput<int>(1)); return; }
        if (t == typeof(long)) { SetOutput(0, GetInput<long>(0) >= GetInput<long>(1)); return; }

        if (t == typeof(char)) { SetOutput(0, GetInput<char>(0) >= GetInput<char>(1)); return; }
        if (t == typeof(byte)) { SetOutput(0, GetInput<byte>(0) >= GetInput<byte>(1)); return; }
    }

    public override Node Polymorph(PinWireChange change) =>
        PolymorphHelpers.ToNonGenericIfAllInputsDisconnected<GreaterThanOrEqual>(change);
}
