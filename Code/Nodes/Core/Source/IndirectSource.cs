namespace Nodebox.Nodes;


[Register]
[Description("Continuously reads a property by reference")]
[Tag("Core")]
[Reader]
[Polymorphic]
public class IndirectSource : Node
{
	public override (Pin[] In, Pin[] Out) InitialPins => (
        new Pin[] {
            Pin.New<Reference>("Reference"),
        },

        new Pin[] {
            Pin.New<object>("*"),
        }
    );

    public IndirectSource() {
        SetOutput<object>(0, null);
    }

	public override void Evaluate() {
        var reference = GetInput<Reference>(0);
        RenamePin(PinType.Output, 0, reference?.PropertyName ?? "???");
        if (reference == null) return;
        //SetPinType(PinType.Input, 0, reference.TargetType); // ???
		if (!reference.TryRead(out var value)) return;
        SetOutput(0, value);
	}

	public override Node Polymorph(PinWireChange change) {
        Type type = change.PolymorphTargetType;
        if (change.PinType == PinType.Input) {
            if (!TypeLibrary.GetType(type).IsGenericType) {
                return null;
            }

            type = TypeLibrary.GetGenericArguments(type)[0]; // Dereference type or smth lmao
        }

        return PolymorphHelpers.ToType(typeof(IndirectSource<>), type);
    }
}


[Register(typeof(Library.All))]
[Polymorphic(typeof(IndirectSource))]
public class IndirectSource<T> : IndirectSource
{
	public override (Pin[] In, Pin[] Out) InitialPins => (
        new Pin[] {
            Pin.New<Reference<T>>("Reference"),
        },

        new Pin[] {
            Pin.New<T>("*"),
        }
    );

    public IndirectSource() {
        SetOutput<T>(0, default);
    }

	public override void Evaluate() {
        var reference = GetInput<Reference<T>>(0);
        RenamePin(PinType.Output, 0, reference?.PropertyName ?? "???");
        if (reference == null) return;
		if (!reference.TryRead(out var value)) return;
        SetOutput(0, value);
	}

	public override Node Polymorph(PinWireChange change) =>
        PolymorphHelpers.ToNonGenericIfAllDisconnected<IndirectSource>(change);
}
