
namespace Nodebox.Nodes;

[Register]
[Tag("Math", "Trigonometry")]
[Alias("Sine")]
[Polymorphic(true)]
public class Sin : Node
{
    public override (Pin[] In, Pin[] Out) InitialPins => (
        new Pin[] {
            Pin.New<Polymorphic>("X"),
        },
        
        new Pin[] {
            Pin.New<Polymorphic>("Sin(X)"),
        }
    );

	public override Node Polymorph(PinWireChange change) =>
        PolymorphHelpers.ToConnectedTypeIfRegistered(typeof(Sin<>), change);
}


[Register(typeof(Library.Float))]
[Polymorphic(typeof(Sin))]
public class Sin<T> : Sin
{
    public override (Pin[] In, Pin[] Out) InitialPins => base.InitialPins.SubstitutePolymorphic<T>();

    public override void Evaluate() {
        if (typeof(T) == typeof(float)) {
            SetOutput(0, MathF.Sin(GetInput<float>(0)));
            return;
        }

        if (typeof(T) == typeof(double)) {
            SetOutput(0, Math.Sin(GetInput<double>(0)));
            return;
        }

        throw new NotImplementedException();
    }

    public override Node Polymorph(PinWireChange change) =>
        PolymorphHelpers.ToNonGenericIfAllDisconnected<Sin>(change);
}
