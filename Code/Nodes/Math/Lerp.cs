using Nodebox.Util;

namespace Nodebox.Nodes;

internal static class LerpHelpers {
    public static (Pin[] In, Pin[] Out) InitialPinsEx<T>() => (
        new Pin[] {
            Pin.New<T>("From"),
            Pin.New<T>("To"),
            new(typeof(T) == typeof(double) ? typeof(double) : typeof(float), "Frac"),
        },
        
        new Pin[] {
            Pin.New<T>("*"),
        }
    );

    public static void EvaluateEx<T>(Node self, bool clamped) {
        if (typeof(T) == typeof(float)) {
            self.SetOutput(0, self.GetInput<float>(0).LerpTo(self.GetInput<float>(1), self.GetInput<float>(2), clamped));
            return;
        }
        
        if (typeof(T) == typeof(double)) {
            self.SetOutput(0, self.GetInput<double>(0).LerpTo(self.GetInput<double>(1), self.GetInput<double>(2), clamped));
            return;
        }

        if (typeof(T) == typeof(Vector2)) {
            self.SetOutput(0, self.GetInput<Vector2>(0).LerpTo(self.GetInput<Vector2>(1), self.GetInput<float>(2), clamped));
            return;
        }

        if (typeof(T) == typeof(Vector3)) {
            self.SetOutput(0, self.GetInput<Vector3>(0).LerpTo(self.GetInput<Vector3>(1), self.GetInput<float>(2), clamped));
            return;
        }
        
        if (typeof(T) == typeof(Vector4)) {
            self.SetOutput(0, self.GetInput<Vector4>(0).LerpTo(self.GetInput<Vector4>(1), self.GetInput<float>(2), clamped));
            return;
        }


        if (typeof(T) == typeof(Rotation)) {
            self.SetOutput(0, self.GetInput<Rotation>(0).LerpTo(self.GetInput<Rotation>(1), self.GetInput<float>(2), clamped));
        }
        

        if (typeof(T) == typeof(Color)) {
            self.SetOutput(0, self.GetInput<Color>(0).LerpTo(self.GetInput<Color>(1), self.GetInput<float>(2), clamped));
        }


        throw new NotImplementedException();
    }

    public static Node PolymorphEx(PinWireChange change, bool clamped) {
        if (change.PolymorphTargetType == null) {
            return null;
        }

        var target = clamped ? typeof(Lerp<>) : typeof(LerpUnclamped<>);
        if (change.PinIndex == 2) {
            if (change.PolymorphTargetType == typeof(double)) {
                return PolymorphHelpers.ToType(target, typeof(double));
            }

            return null;
        }

        return PolymorphHelpers.ToConnectedTypeIfRegistered(target, change);
    }
}


[Register]
[Tag("Math")]
[Alias("Interpolate", "Interpolation")]
[Polymorphic(true)]
public class Lerp : Node
{
    public override (Pin[] In, Pin[] Out) InitialPins => LerpHelpers.InitialPinsEx<Polymorphic>();
	public override Node Polymorph(PinWireChange change) => LerpHelpers.PolymorphEx(change, true);
}

[Register(typeof(Library.FloatN))]
[Register(typeof(Rotation))]
[Register(typeof(Color))]
[Polymorphic(typeof(Lerp))]
public class Lerp<T> : Lerp
{
    public override (Pin[] In, Pin[] Out) InitialPins => LerpHelpers.InitialPinsEx<T>();
	public override void Evaluate() => LerpHelpers.EvaluateEx<T>(this, true);
	public override Node Polymorph(PinWireChange change) =>
        PolymorphHelpers.ToNonGenericIfAllDisconnected<Lerp>(change);
}



[Register]
[Tag("Math")]
[Alias("InterpolateClamped", "InterpolationClamped")]
[Polymorphic(true)]
public class LerpUnclamped : Lerp
{
	public override Node Polymorph(PinWireChange change) => LerpHelpers.PolymorphEx(change, false);
}


[Register(typeof(Library.FloatN))]
[Register(typeof(Rotation))]
[Register(typeof(Color))]
[Polymorphic(typeof(LerpUnclamped))]
public class LerpUnclamped<T> : LerpUnclamped
{
    public override (Pin[] In, Pin[] Out) InitialPins => LerpHelpers.InitialPinsEx<T>();
	public override void Evaluate() => LerpHelpers.EvaluateEx<T>(this, false);
	public override Node Polymorph(PinWireChange change) =>
        PolymorphHelpers.ToNonGenericIfAllDisconnected<LerpUnclamped>(change);
}
