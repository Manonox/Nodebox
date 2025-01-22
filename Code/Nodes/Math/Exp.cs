namespace Nodebox.Nodes;


[Register]
[Description("Returns constant E to the power of X")]
[Tag("Math")]
[Polymorphic(true)]
public class Exp : Node
{
    public override (Pin[] In, Pin[] Out) InitialPins => (
        new Pin[] {
            Pin.New<Polymorphic>("X")
        },
        
        new Pin[] {
            Pin.New<Polymorphic>("E to the X")
        }
    );

	public override Node Polymorph(PinWireChange change) =>
        PolymorphHelpers.ToConnectedTypeIfRegistered(typeof(Exp<>), change);
}


[Register(typeof(Library.Float))]
[Polymorphic(typeof(Exp))]
public class Exp<T> : Exp
{
    public override (Pin[] In, Pin[] Out) InitialPins => base.InitialPins.SubstitutePolymorphic<T>();

    public override void Evaluate() {
        if (typeof(T) == typeof(float)) {
            SetOutput(0, MathF.Exp(GetInput<float>(0)));
            return;
        }

        if (typeof(T) == typeof(double)) {
            SetOutput(0, Math.Exp(GetInput<double>(0)));
            return;
        }

        throw new NotImplementedException();
    }

	public override Node Polymorph(PinWireChange change) =>
        PolymorphHelpers.ToNonGenericIfAllDisconnected<Exp>(change);
}

