
namespace Nodebox.Nodes;


[Register]
[Description("Math")]
[Polymorphic(true)]
public class Truncate : Node
{
    public override (Pin[] In, Pin[] Out) InitialPins => (
        new Pin[] {
            Pin.New<Polymorphic>("X"),
        },
        
        new Pin[] {
            Pin.New<Polymorphic>("Truncated X")
        }
    );

	public override Node Polymorph(PinWireChange change) =>
        PolymorphHelpers.ToConnectedTypeIfRegistered(typeof(Truncate<>), change);
}

[Register(typeof(Library.FloatN))]
[Polymorphic(typeof(Truncate))]
public class Truncate<T> : Truncate
{
    public override (Pin[] In, Pin[] Out) InitialPins => base.InitialPins.SubstitutePolymorphic<T>();

    public override void Evaluate() {
        if (typeof(T) == typeof(float)) {
            SetOutput(0, MathF.Truncate(GetInput<float>(0)));
            return;
        }
        
        if (typeof(T) == typeof(double)) {
            SetOutput(0, Math.Truncate(GetInput<double>(0)));
            return;
        }

        if (typeof(T) == typeof(Vector2)) {
            SetOutput(0, GetInput<Vector2>(0).Truncate());
            return;
        }

        if (typeof(T) == typeof(Vector3)) {
            SetOutput(0, GetInput<Vector3>(0).Truncate());
            return;
        }
        
        if (typeof(T) == typeof(Vector4)) {
            SetOutput(0, GetInput<Vector4>(0).Truncate());
            return;
        }

        throw new NotImplementedException();
    }

	public override Node Polymorph(PinWireChange change) =>
        PolymorphHelpers.ToNonGenericIfAllDisconnected<Truncate>(change);
}

