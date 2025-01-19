using System.Collections.Immutable;
using System.Security.Cryptography.X509Certificates;
using Sandbox.Diagnostics;

namespace Nodebox;

public class Wire : IDisposable, IMeta
{
    public Type Type { get; set; }
    
    public WeakReference<Node> From { get; private set; } = new(null);
    public int FromIndex { get; private set; }

    public WeakReference<Node> To { get; private set; } = new(null);
    public int ToIndex { get; private set; }
    
    internal Dictionary<Type, Meta> _meta = new();
    
    public static Dictionary<(Type In, Type Out), Func<object, object>> ImplicitConvertions { get; set; } = new() {
        { (typeof(float), typeof(int)), x => Convert.ToInt32(x) },
        { (typeof(int), typeof(float)), x => Convert.ToSingle(x) },

        { (typeof(int), typeof(double)), value => Convert.ToDouble(value) },
        { (typeof(double), typeof(int)), value => Convert.ToInt32(value) },

        { (typeof(double), typeof(float)), value => Convert.ToSingle(value) },
        { (typeof(float), typeof(double)), value => Convert.ToDouble(value) },

        // Vector types?
    };

    public Wire(Node from, int fromIndex, Node to, int toIndex) {
		Assert.NotNull(from, "From can't be null");
        Assert.NotNull(to, "To can't be null");
		
        Assert.False(fromIndex < 0 | fromIndex >= from.OutputPins.Count, "From Index out of bounds");
        Assert.False(toIndex < 0 | toIndex >= to.InputPins.Count, "To Index out of bounds");

        Assert.False(from == to, "Node cannot be connected to itself");
        
        var fromPin = from.OutputPins[fromIndex];
        var toPin = to.InputPins[toIndex];
        if (toPin.Type != typeof(object)) {
            if (!ImplicitConvertions.TryGetValue((fromPin.Type, toPin.Type), out var _))
                Assert.True(fromPin.Type == toPin.Type, $"can't connect Pin types ({fromPin.Type.GetPrettyName()} -> {toPin.Type.GetPrettyName()})");
        }
        Assert.False(to.InputWires[toIndex].IsValid(), "Recipient already has a wire connected to this index");
        
        Type = fromPin.Type;
        From.SetTarget(from);
        FromIndex = fromIndex;
        To.SetTarget(to);
        ToIndex = toIndex;

        from.OutputWires[fromIndex].Add(new WeakReference<Wire>(this));
        to.InputWires[toIndex] = new WeakReference<Wire>(this);

        Pass();
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

    public void Pass() {
        From.TryGetTarget(out var from);
        To.TryGetTarget(out var to);

        var value = from.OutputValues[FromIndex];
        var inType = from.OutputPins[FromIndex].Type;
        var outType = to.InputPins[ToIndex].Type;
        if (outType != typeof(object) && inType != outType) {
            Assert.True(ImplicitConvertions.TryGetValue((inType, outType), out var convert), "wtf");
            value = convert(value);
        }

        to.SetInput(ToIndex, value);
    }
    
    private bool disposed = false;
    public void Dispose() {
        if (!disposed) {
            Assert.NotNull(From, "should never be null, because it's a WeakReference");
            if (From.TryGetTarget(out var from)) {
                var list = from.OutputWires[FromIndex];
                var immutableList = list.ToImmutableList();
                var index = immutableList.FindIndex(x => {
                    if (!x.TryGetTarget(out var wire))
                        return false;
                    return wire == this;
                });
                if (index >= 0) {
                    list.RemoveAt(index);
                }
            }

            Assert.NotNull(To, "should never be null, because it's a WeakReference");
            if (To.TryGetTarget(out var to)) {
                to.InputWires[ToIndex].SetTarget(null);
            }

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
