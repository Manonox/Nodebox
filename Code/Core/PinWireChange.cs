namespace Nodebox;

public record PinWireChange {
    public PinWireChange(Node node, PinType pinType, int pinIndex, Wire wire) {
        Target = node;
        PinType = pinType;
        PinIndex = pinIndex;
        Wire = wire;
    }

    public Node Target { get; private set; }
    public PinType PinType { get; private set; }
    public int PinIndex { get; private set; }
    public Wire Wire { get; private set; } // null if disconnected

    public Pin Pin => Target.GetPin(PinType, PinIndex);
    public bool IsPinPolymorphic => Pin.Type == typeof(Polymorphic);

    public bool HasPolymorphType { get {
        return Wire?.FromType == typeof(Polymorphic) || Wire?.ToType == typeof(Polymorphic);
    } }


    public Type PolymorphTargetType { get {
        var fromType = Wire?.FromType;
        if (fromType != typeof(Polymorphic)) return fromType;
        return Wire?.ToType;
    } }
}