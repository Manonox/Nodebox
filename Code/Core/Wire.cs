using Sandbox.Diagnostics;

namespace Nodebox;

public sealed class Wire : IDisposable, IMeta
{
    public Type Type { get; set; }
    
    public WeakReference<Node> From { get; private set; }
    public int FromIndex { get; private set; }

    public WeakReference<Node> To { get; private set; }
    public int ToIndex { get; private set; }
    

    internal Dictionary<Type, Meta> _meta = new();
    
    public Wire(Node from, int fromIndex, Node to, int toIndex) {
        Assert.False(from == to, "Node cannot be connected to itself");

        var fromPin = from._outputPins[fromIndex];
        var toPin = to._inputPins[toIndex];
        if (toPin.Type != typeof(object))
            Assert.True(fromPin.Type == toPin.Type, "Connected Pin types cannot be different");
        Assert.False(to._inputWires[toIndex].IsValid(), "Recipient already has a wire connected to this index");
        
        Type = fromPin.Type;
        From = new WeakReference<Node>(from);
        FromIndex = fromIndex;
        To = new WeakReference<Node>(to);
        ToIndex = toIndex;

        from._outputWires[fromIndex].Add(new WeakReference<Wire>(this));
        to._inputWires[toIndex] = new WeakReference<Wire>(this);
    }
    
    public T GetMeta<T>() {
        _meta.TryGetValue(typeof(T), out Meta value);
        return ((Meta<T>)value).Value;
    }

    public void SetMeta<T>(T value) {
        _meta.Add(typeof(T), new Meta<T>(value));
    }

	public override string ToString() {
        From.TryGetTarget(out var from);
        To.TryGetTarget(out var to);
		return $"{from}[{FromIndex}] -> {to}[{ToIndex}]";
	}
    
    private bool disposed = false;
    public void Dispose() {
        if (!disposed) {
            if (From.TryGetTarget(out var from)) {
                from._outputWires[FromIndex] = null;
            }

            if (To.TryGetTarget(out var to)) {
                to._inputWires[ToIndex] = null;
            }

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
