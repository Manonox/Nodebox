namespace Nodebox.Nodes;

[Register]
[Tag("Operator")]
[Alias("==")]
[Polymorphic(true)]
public class Equal : Node
{
    public override (Pin[] In, Pin[] Out) InitialPins => (
        new Pin[] {
            Pin.New<Polymorphic>("A"),
            Pin.New<Polymorphic>("B"),
        },
        
        new Pin[] {
            Pin.New<bool>("A == B")
        }
    );

	public override Node Polymorph(PinWireChange change) =>
        PolymorphHelpers.ToConnectedInputTypeIfRegistered(typeof(Equal<>), change);
}


[Register(typeof(Library.All))]
[Polymorphic(typeof(Equal))]
public class Equal<T> : Equal
{
    public override (Pin[] In, Pin[] Out) InitialPins => base.InitialPins.SubstitutePolymorphic<T>();
    public override void Evaluate() {
        SetOutput(0, GetInput<T>(0).Equals(GetInput<T>(1)));
    }

    public override Node Polymorph(PinWireChange change) =>
        PolymorphHelpers.ToNonGenericIfAllInputsDisconnected<Equal>(change);
}
