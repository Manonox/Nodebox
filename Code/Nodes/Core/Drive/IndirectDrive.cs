
namespace Nodebox.Nodes;

[Register]
[Description("Continuously writes to a property by reference")]
[Tag("Core")]
[Writer]
[Polymorphic]
public class IndirectDrive : Node
{
	public override (Pin[] In, Pin[] Out) InitialPins => (
        new Pin[] {
            Pin.New<Reference>("Reference"),
            Pin.New<object>("*")
        },

        new Pin[] {
        }
    );

	public override void Evaluate() {
		var reference = GetInput<Reference>(0);
        RenamePin(PinType.Input, 1, reference?.PropertyName ?? "???");
        if (reference == null) return;
        //SetPinType(PinType.Input, 0, reference.TargetType); // ???
        reference.Write(GetInput<object>(1));
	}

	public override Node Polymorph(PinWireChange change) {
        Type type = change.PolymorphTargetType;
        if (change.PinIndex == 0) {
            if (!TypeLibrary.GetType(type).IsGenericType) {
                return null;
            }

            type = TypeLibrary.GetGenericArguments(type)[0]; // Dereference type or smth lmao
        }

        return PolymorphHelpers.ToType(typeof(IndirectDrive<>), type);
    }
}

[Register(typeof(Library.All))]
[Polymorphic(typeof(IndirectDrive))]
public class IndirectDrive<T> : IndirectDrive
{   
	public override (Pin[] In, Pin[] Out) InitialPins => (
        new Pin[] {
            Pin.New<Reference<T>>($"Reference<{typeof(T).GetDisplayName()}>"),
            Pin.New<T>("*")
        },

        new Pin[] {
        }
    );

	public override void Evaluate() {
		var reference = GetInput<Reference<T>>(0);
        RenamePin(PinType.Input, 1, reference?.PropertyName ?? "???");
        if (reference == null) return;
        reference.Write(GetInput<T>(1));
	}

	public override Node Polymorph(PinWireChange change) =>
        PolymorphHelpers.ToNonGenericIfAllDisconnected<IndirectDrive>(change);
}
