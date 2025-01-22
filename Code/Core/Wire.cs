namespace Nodebox;

public class Wire : IDisposable, IMeta
{   
    private readonly WeakReference<Node> _from = new(null);
    public Node From { get => _from.GetTarget(); set => _from.SetTarget(value); }
    public int FromIndex { get; private set; }

    private readonly WeakReference<Node> _to = new(null);
    public Node To { get => _to.GetTarget(); set => _to.SetTarget(value); }
    public int ToIndex { get; private set; }
    
    public NetDictionary<Type, Meta> Meta { get; set; } = new();

    
    public Pin FromPin => From.OutputPins[FromIndex];
    public Pin ToPin => To.InputPins[ToIndex];

    public Type FromType => FromPin.Type;
    public Type ToType => ToPin.Type;

    public Wire(Node from, int fromIndex, Node to, int toIndex, bool unconnected = false) {
		Check(from, fromIndex, to, toIndex);

        From = from;
        FromIndex = fromIndex;
        To = to;
        ToIndex = toIndex;

        if (unconnected) return;
        Connect();
    }

    public void Connect() {
        From.SetOutputWire(FromIndex, this);
        To.SetInputWire(ToIndex, this);

        Pass();
    }

    public static void Check(Node from, int fromIndex, Node to, int toIndex) {
        Assert.NotNull(from, "From can't be null");
        Assert.NotNull(to, "To can't be null");
		
        Assert.False(fromIndex < 0 | fromIndex >= from.OutputPins.Count, "From Index out of bounds");
        Assert.False(toIndex < 0 | toIndex >= to.InputPins.Count, "To Index out of bounds");

        Assert.False(from == to, "Node cannot be connected to itself");
        Assert.False(to.InputWires[toIndex].IsValid(), "Recipient already has a wire connected to this index");
        
        var fromType = from.OutputPins[fromIndex].Type;
        var toType = to.InputPins[toIndex].Type;

        if (fromType == typeof(object) || toType == typeof(object)) return;
        Assert.False(fromType == typeof(Polymorphic) && toType == typeof(Polymorphic), "Both pins can't be Polymorphic");
        if (fromType == typeof(Polymorphic) || toType == typeof(Polymorphic)) return;
        if (Library.TryGetImplicitConversion(fromType, toType, out var _)) return;

        Assert.True(fromType == toType, $"can't connect Pin types ({fromType.GetDisplayName()} -> {toType.GetDisplayName()})");
    }

    
    public T GetMeta<T>() {
        if (!Meta.TryGetValue(typeof(T), out Meta value))
            return default;
        return ((Meta<T>)value).Value;
    }

    public bool TryGetMeta<T>(out T value) {
        value = default;
        var result = Meta.TryGetValue(typeof(T), out Meta meta);
        if (result)
            value = ((Meta<T>)meta).Value;
        return result;
    }

    public void SetMeta<T>(T value) {
        Meta.Add(typeof(T), new Meta<T>(value));
    }

    public void Pass() {
        var value = From.OutputValues[FromIndex];
        
        var inType = From.OutputPins[FromIndex].Type;
        var outType = To.InputPins[ToIndex].Type;

		void done() => To.SetInput( ToIndex, value );

		if (inType == typeof(object) || outType == typeof(object)) {
            done();
            return;
        }

        Assert.False(inType == typeof(Polymorphic) && outType == typeof(Polymorphic), "wtf");
        if (inType == typeof(Polymorphic) || outType == typeof(Polymorphic)) {
            done();
            return;
        }
        
        if (inType == outType) {
            done();
            return;
        }

        Assert.True(Library.TryGetImplicitConversion(inType, outType, out var convert), "wtf");
        value = convert(value);
        To.SetInput(ToIndex, value);
    }
    
	public override string ToString() {
		return $"{From}[{FromIndex}] -> {To}[{ToIndex}]";
	}
    
    private bool disposed = false;
    public void Dispose() {
        if (!disposed) {
            From?.UnsetOutputWire(FromIndex, this);
            To?.UnsetInputWire(ToIndex);

            //Log.Info(("Wire destroyed", From, FromIndex, To, ToIndex));

            disposed = true;
        }

        GC.SuppressFinalize(this);
    }

    ~Wire() {
        Dispose();
    }
}

public static class WireExtensions {
    public static bool IsValid(this WeakReference<Wire> wire) {
        return wire != null && wire.TryGetTarget(out var target) && target != null;
    }
}
