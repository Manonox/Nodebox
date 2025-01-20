namespace Nodebox.Nodes;


[RegisterNode]
public class IndirectSource : Node
{
    public override bool Tick => true;
    public override string Name => "IndirectSource";
    public override string Desc => "Continuously reads a property by reference";
    public override string[] Groups => new string[] { "Core" };

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
        //SetPinType(PinType.Input, 0, reference.TargetType);
		if (!reference.TryRead(out var value)) return;
        SetOutput(0, value);
	}
}


[RegisterNode]
public class IndirectSource<T> : IndirectSource
{
    public override Type[] Generics => [typeof(T)];

    public override bool Tick => true;
    public override string Name => "IndirectSource";

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
}
