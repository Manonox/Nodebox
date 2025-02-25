using Nodebox.Util;

namespace Nodebox.Nodes;

[Register]
[Description("Convert degrees to radians")]
[Tag("Math", "Trigonometry")]
[Alias("Deg2Rad", "DegreesToRadians", "Degrees2Radians", "DegreeToRadian", "Degree2Radian")]
[Polymorphic(true)]
public class DegToRad : Node
{
    public override (Pin[] In, Pin[] Out) InitialPins => (
        new Pin[] {
            Pin.New<Polymorphic>("Degrees"),
        },
        
        new Pin[] {
            Pin.New<Polymorphic>("Radians"),
        }
    );

	public override Node Polymorph(PinWireChange change) =>
        PolymorphHelpers.ToConnectedTypeIfRegistered(typeof(DegToRad<>), change);
}


[Register(typeof(Library.Float))]
[Polymorphic(typeof(DegToRad))]
public class DegToRad<T> : DegToRad
{
    public override (Pin[] In, Pin[] Out) InitialPins => base.InitialPins.SubstitutePolymorphic<T>();

    public override void Evaluate() {
        if (typeof(T) == typeof(float)) {
            SetOutput(0, GetInput<float>(0).DegreeToRadian());
            return;
        }

        if (typeof(T) == typeof(double)) {
            SetOutput(0, GetInput<double>(0).DegreeToRadian());
            return;
        }

        throw new NotImplementedException();
    }

    public override Node Polymorph(PinWireChange change) =>
        PolymorphHelpers.ToNonGenericIfAllDisconnected<DegToRad>(change);
}