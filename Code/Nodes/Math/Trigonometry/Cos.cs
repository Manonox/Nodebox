
namespace Nodebox.Nodes;

[Register]
[Tag("Math", "Trigonometry")]
[Alias("Cosine")]
[Polymorphic(true)]
public class Cos : Node
{
    public override (Pin[] In, Pin[] Out) InitialPins => (
        new Pin[] {
            Pin.New<Polymorphic>("X"),
        },
        
        new Pin[] {
            Pin.New<Polymorphic>("Cos(X)"),
        }
    );

	public override Node Polymorph(PinWireChange change) =>
        PolymorphHelpers.ToConnectedTypeIfRegistered(typeof(Cos<>), change);
}


[Register(typeof(Library.Float))]
[Polymorphic(typeof(Cos))]
public class Cos<T> : Cos
{
    public override (Pin[] In, Pin[] Out) InitialPins => base.InitialPins.SubstitutePolymorphic<T>();

    public override void Evaluate() {
        if (typeof(T) == typeof(float)) {
            SetOutput(0, MathF.Cos(GetInput<float>(0)));
            return;
        }

        if (typeof(T) == typeof(double)) {
            SetOutput(0, Math.Cos(GetInput<double>(0)));
            return;
        }

        throw new NotImplementedException();
    }

    public override Node Polymorph(PinWireChange change) =>
        PolymorphHelpers.ToNonGenericIfAllDisconnected<Cos>(change);
}
