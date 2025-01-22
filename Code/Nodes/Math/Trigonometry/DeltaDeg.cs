using Nodebox.Util;

namespace Nodebox.Nodes;


[Register]
[Description("Gets the difference between 2 angles in degrees (not `Angles`)")]
[Tag("Math", "Trigonometry")]
[Alias("DifferenceAngles", "DifferenceDegrees")]
[Polymorphic(true)]
public class DeltaDeg : Node
{
    public override (Pin[] In, Pin[] Out) InitialPins => (
        new Pin[] {
            Pin.New<Polymorphic>("From"),
            Pin.New<Polymorphic>("To"),
        },
        
        new Pin[] {
            Pin.New<Polymorphic>("Degrees"),
        }
    );

	public override Node Polymorph(PinWireChange change) =>
        PolymorphHelpers.ToConnectedTypeIfRegistered(typeof(DeltaDeg<>), change);
}

[Register(typeof(Library.Float))]
[Polymorphic(typeof(DeltaDeg))]
public class DeltaDeg<T> : DeltaDeg
{
    public override (Pin[] In, Pin[] Out) InitialPins => base.InitialPins.SubstitutePolymorphic<T>();
    public override void Evaluate() {
        if (typeof(T) == typeof(float)) {
            SetOutput(0, MathX.DeltaDegrees(GetInput<float>(0), GetInput<float>(1)));
            return;
        }

        if (typeof(T) == typeof(double)) {
            SetOutput(0, MathOmega.DeltaDegrees(GetInput<double>(0), GetInput<double>(1)));
            return;
        }

        throw new NotImplementedException();
    }

	public override Node Polymorph(PinWireChange change) =>
        PolymorphHelpers.ToNonGenericIfAllDisconnected<DeltaDeg>(change);
}
