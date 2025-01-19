namespace Nodebox.Nodes;


[RegisterNode]
public class IndirectDrive<T> : Node
{
    public override Type[] Generics => [typeof(T)];
    
    public override bool Tick => true;
    public override string Name => "IndirectDrive";
    public override string Desc => "Continuously writes to a property by reference";
    public override string[] Groups => new string[] { "Core" };

	public override (Pin[] In, Pin[] Out) InitialPins => (
        new Pin[] {
            Pin.New<Reference<T>>("Reference"),
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
}
