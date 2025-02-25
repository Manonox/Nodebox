using Nodebox.Util;

namespace Nodebox.Nodes;

[Register]
[Description("Convert radians to degrees")]
[Tag("Math", "Trigonometry")]
[Alias("Rad2Deg", "RadiansToDegrees", "Radians2Degrees", "RadianToDegree", "Radian2Degree")]
[Polymorphic(true)]
public class RadToDeg : Node
{
    public override (Pin[] In, Pin[] Out) InitialPins => (
        new Pin[] {
            Pin.New<Polymorphic>("Radians"),
        },
        
        new Pin[] {
            Pin.New<Polymorphic>("Degrees"),
        }
    );

	public override Node Polymorph(PinWireChange change) =>
        PolymorphHelpers.ToConnectedTypeIfRegistered(typeof(RadToDeg<>), change);
}


[Register(typeof(Library.Float))]
[Polymorphic(typeof(RadToDeg))]
public class RadToDeg<T> : RadToDeg
{
    public override (Pin[] In, Pin[] Out) InitialPins => base.InitialPins.SubstitutePolymorphic<T>();

    public override void Evaluate() {
        if (typeof(T) == typeof(float)) {
            SetOutput(0, GetInput<float>(0).RadianToDegree());
            return;
        }

        if (typeof(T) == typeof(double)) {
            SetOutput(0, GetInput<double>(0).RadianToDegree());
            return;
        }

        throw new NotImplementedException();
    }

    public override Node Polymorph(PinWireChange change) =>
        PolymorphHelpers.ToNonGenericIfAllDisconnected<RadToDeg>(change);
}
