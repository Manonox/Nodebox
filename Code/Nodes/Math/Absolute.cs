
namespace Nodebox.Nodes;

[Register]
[Description("Returns the absolute value of a number (turns negative into positive)\n**Does the same component-wise for vectors**")]
[Tag("Math")]
[Polymorphic(true)]
public class Absolute : Node
{
    public override (Pin[] In, Pin[] Out) InitialPins => (
        new Pin[] {
            Pin.New<Polymorphic>("X"),
        },
        
        new Pin[] {
            Pin.New<Polymorphic>("|X|")
        }
    );

	public override Node Polymorph(PinWireChange change) =>
        PolymorphHelpers.ToConnectedTypeIfRegistered(typeof(Absolute<>), change);
}

[Register(typeof(Library.Integer))]
[Register(typeof(Library.Float))]
[Register(typeof(Library.Vector))]
[Register(typeof(Color))]
[Polymorphic(typeof(Absolute))]
public class Absolute<T> : Absolute
{
    public override (Pin[] In, Pin[] Out) InitialPins => (
        new Pin[] {
            Pin.New<T>("X"),
        },
        
        new Pin[] {
            Pin.New<T>(!typeof(T).IsVectorType() ? "|X|" : "Abs(X)")
        }
    );

    public override void Evaluate() {
        if (typeof(T) == typeof(float)) {
            SetOutput(0, Math.Abs(GetInput<float>(0)));
            return;
        }
        
        if (typeof(T) == typeof(double)) {
            SetOutput(0, Math.Abs(GetInput<double>(0)));
            return;
        }

        if (typeof(T) == typeof(int)) {
            SetOutput(0, Math.Abs(GetInput<int>(0)));
            return;
        }
        
        if (typeof(T) == typeof(byte)) {
            SetOutput(0, Math.Abs(GetInput<byte>(0)));
            return;
        }

        if (typeof(T) == typeof(char)) {
            SetOutput(0, Math.Abs(GetInput<char>(0)));
            return;
        }

        if (typeof(T) == typeof(long)) {
            SetOutput(0, Math.Abs(GetInput<long>(0)));
            return;
        }
        
        if (typeof(T) == typeof(Vector2)) {
            SetOutput(0, GetInput<Vector2>(0).Abs());
            return;
        }
        
        if (typeof(T) == typeof(Vector3)) {
            SetOutput(0, GetInput<Vector3>(0).Abs());
            return;
        }
        
        if (typeof(T) == typeof(Vector4)) {
            SetOutput(0, GetInput<Vector4>(0).Abs());
            return;
        }
        
        if (typeof(T) == typeof(Vector2Int)) {
            SetOutput(0, GetInput<Vector2Int>(0).Abs());
            return;
        }
        
        if (typeof(T) == typeof(Vector3Int)) {
            SetOutput(0, GetInput<Vector3Int>(0).Abs());
            return;
        }

        if (typeof(T) == typeof(Color)) {
            SetOutput(0, GetInput<Color>(0).Abs());
            return;
        }

        throw new NotImplementedException();
    }

	public override Node Polymorph(PinWireChange change) =>
        PolymorphHelpers.ToNonGenericIfAllDisconnected<Absolute>(change);
}

