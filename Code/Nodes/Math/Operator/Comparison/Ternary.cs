namespace Nodebox.Nodes;

[Register]
[Tag("Operator")]
[Alias("?:", "Switch")]
[Polymorphic(true)]
public class Ternary : Node
{
    public override (Pin[] In, Pin[] Out) InitialPins => (
        new Pin[] {
            Pin.New<Polymorphic>("IfTrue"),
            Pin.New<Polymorphic>("IfFalse"),
            Pin.New<bool>("Condition"),
        },
        
        new Pin[] {
            Pin.New<Polymorphic>("*")
        }
    );

	public override Node Polymorph(PinWireChange change) {
        if (change.PinIndex == 2) return null;
        return PolymorphHelpers.ToConnectedTypeIfRegistered(typeof(Ternary<>), change);
    }
}


[Register(typeof(Library.All))]
[Polymorphic(typeof(Ternary))]
public class Ternary<T> : Ternary
{
    public override (Pin[] In, Pin[] Out) InitialPins => base.InitialPins.SubstitutePolymorphic<T>();
    public override void Evaluate() {
        SetOutput(0, GetInput<bool>(2) ? GetInput<T>(0) : GetInput<T>(1));
    }

    public override Node Polymorph(PinWireChange change) {
        if (change.Target.GetOutputWires().Any()) return null;
        if (change.Target.GetInputWires().Where(x => x.ToIndex != 2).Any()) return null;
        return PolymorphHelpers.ToNonGeneric<Ternary>();
    }
}
