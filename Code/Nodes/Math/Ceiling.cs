
namespace Nodebox.Nodes;


[Register]
[Description("Rounds the value up")]
[Tag("Math")]
[Polymorphic(true)]
public class Ceiling : Node
{
    public override (Pin[] In, Pin[] Out) InitialPins => (
        new Pin[] {
            Pin.New<Polymorphic>("X"),
        },
        
        new Pin[] {
            Pin.New<Polymorphic>("⌈X⌉")
        }
    );

	public override Node Polymorph(PinWireChange change) =>
        PolymorphHelpers.ToConnectedTypeIfRegistered(typeof(Ceiling<>), change);
}


[Register(typeof(Library.FloatN))]
[Polymorphic(typeof(Ceiling))]
public class Ceiling<T> : Ceiling
{
    public override (Pin[] In, Pin[] Out) InitialPins => base.InitialPins.SubstitutePolymorphic<T>();

    public override void Evaluate() {
        if (typeof(T) == typeof(float)) {
            SetOutput(0, MathF.Ceiling(GetInput<float>(0)));
            return;
        }
        
        if (typeof(T) == typeof(double)) {
            SetOutput(0, Math.Ceiling(GetInput<double>(0)));
            return;
        }

        if (typeof(T) == typeof(Vector2)) {
            SetOutput(0, GetInput<Vector2>(0).Ceiling());
            return;
        }

        if (typeof(T) == typeof(Vector3)) {
            SetOutput(0, GetInput<Vector3>(0).Ceiling());
            return;
        }
        
        if (typeof(T) == typeof(Vector4)) {
            SetOutput(0, GetInput<Vector4>(0).Ceiling());
            return;
        }

        throw new NotImplementedException();
    }
    
	public override Node Polymorph(PinWireChange change) =>
        PolymorphHelpers.ToNonGenericIfAllDisconnected<Ceiling>(change);
}

