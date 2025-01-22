using Nodebox.Util;

namespace Nodebox.Nodes;

[Register]
[Description("Raises A to the power of B")]
[Tag("Math", "Operator")]
[Alias("^", "**", "Exponentiate")]
[Polymorphic(true)]
public class Power : Node
{
    public override (Pin[] In, Pin[] Out) InitialPins => (
        new Pin[] {
            Pin.New<Polymorphic>("A"),
            Pin.New<Polymorphic>("B"),
        },
        
        new Pin[] {
            Pin.New<Polymorphic>("A pow B")
        }
    );

	public override Node Polymorph(PinWireChange change) =>
        PolymorphHelpers.ToConnectedTypeIfRegistered(typeof(Power<>), change);
}

[Register(typeof(Library.Float))]
[Register(typeof(Library.Integer))]
[Polymorphic(typeof(Power))]
public class Power<T> : Power
{
    public override (Pin[] In, Pin[] Out) InitialPins => (
        new Pin[] {
            Pin.New<T>("A"),
            Pin.New<T>("B"),
        },
        
        new Pin[] {
            Pin.New<T>("A pow B")
        }
    );

    public override void Evaluate() {
        var t = typeof(T);

        if (t == typeof(float)) { SetOutput(0, GetInput<float>(0).Pow(GetInput<float>(1))); return; }
        if (t == typeof(double)) { SetOutput(0, GetInput<double>(0).Pow(GetInput<double>(1))); return; }

        if (t == typeof(int)) { SetOutput(0, GetInput<int>(0).Pow(GetInput<int>(1))); return; }
        if (t == typeof(long)) { SetOutput(0, GetInput<long>(0).Pow((int)GetInput<long>(1))); return; }

        throw new NotImplementedException();
    }

	public override Node Polymorph(PinWireChange change) =>
        PolymorphHelpers.ToNonGenericIfAllDisconnected<Power>(change);
}

