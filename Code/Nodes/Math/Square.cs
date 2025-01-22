namespace Nodebox.Nodes;


[Register]
[Tag("Math")]
[Polymorphic(true)]
public class Square : Node
{
    public override (Pin[] In, Pin[] Out) InitialPins => (
        new Pin[] {
            Pin.New<Polymorphic>("X")
        },
        
        new Pin[] {
            Pin.New<Polymorphic>("X²")
        }
    );

	public override Node Polymorph(PinWireChange change)
        => PolymorphHelpers.ToConnectedTypeIfRegistered(typeof(Square<>), change);
}


[Register(typeof(Library.Float))]
[Polymorphic(typeof(Square))]
public class Square<T> : Square
{
    public override (Pin[] In, Pin[] Out) InitialPins => base.InitialPins.SubstitutePolymorphic<T>();

    public override void Evaluate() {
        if (typeof(T) == typeof(float)) {
            var x = GetInput<float>(0);
            SetOutput(0, x * x);
            return;
        }

        if (typeof(T) == typeof(double)) {
            var x = GetInput<double>(0);
            SetOutput(0, x * x);
            return;
        }

        throw new NotImplementedException();
    }

	public override Node Polymorph(PinWireChange change) =>
        PolymorphHelpers.ToNonGenericIfAllDisconnected<Square>(change);
}

