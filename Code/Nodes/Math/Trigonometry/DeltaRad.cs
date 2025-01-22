using Nodebox.Util;

namespace Nodebox.Nodes;


[Register]
[Description("Gets the difference between 2 angles in radians (not `Angles`)")]
[Tag("Math", "Trigonometry")]
[Alias("DifferenceAngles", "DifferenceRadians")]
[Polymorphic(true)]
public class DeltaRad : Node
{
    public override (Pin[] In, Pin[] Out) InitialPins => (
        new Pin[] {
            Pin.New<Polymorphic>("From"),
            Pin.New<Polymorphic>("To"),
        },
        
        new Pin[] {
            Pin.New<Polymorphic>("Radians"),
        }
    );

	public override Node Polymorph(PinWireChange change) =>
        PolymorphHelpers.ToConnectedTypeIfRegistered(typeof(DeltaRad<>), change);
}

[Register(typeof(Library.Float))]
[Polymorphic(typeof(DeltaRad))]
public class DeltaRad<T> : DeltaRad
{
    public override (Pin[] In, Pin[] Out) InitialPins => base.InitialPins.SubstitutePolymorphic<T>();
    public override void Evaluate() {
        if (typeof(T) == typeof(float)) {
            SetOutput(0, MathX.DeltaRadians(GetInput<float>(0), GetInput<float>(1)));
            return;
        }

        if (typeof(T) == typeof(double)) {
            SetOutput(0, MathOmega.DeltaRadians(GetInput<double>(0), GetInput<double>(1)));
            return;
        }

        throw new NotImplementedException();
    }

	public override Node Polymorph(PinWireChange change) =>
        PolymorphHelpers.ToNonGenericIfAllDisconnected<DeltaRad>(change);
}
