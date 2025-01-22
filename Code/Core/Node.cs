using System.Collections.Immutable;

namespace Nodebox;


public class Node : IMeta, IDisposable //, ICloneable
{

// Events
    public delegate void PinWireChangeEventHandler(PinWireChange change);
    public event PinWireChangeEventHandler PinWireChangeEvent;

    public delegate void PolymorphEventHandler(Node node);
    public event PolymorphEventHandler PolymorphEvent;


// Info

    public string Name => DisplayInfo.Name;
    public string Description => DisplayInfo.Description;
    public string Icon => DisplayInfo.Icon;
    public string[] Tags => DisplayInfo.Tags;
    public string[] Aliases => DisplayInfo.Alias;
    public DisplayInfo DisplayInfo { get {
        var displayInfo = DisplayInfo.ForType(GetType(), true);
        // if (NameOverride != null)
        //     displayInfo.Name = NameOverride;
        // if (DescriptionOverride != null)
        //     displayInfo.Description = DescriptionOverride;
        return displayInfo;
    } }

    public bool IsPolymorphic => TypeLibrary.HasAttribute<PolymorphicAttribute>(GetType());
    public bool IsPolymorphRequired => IsPolymorphic && TypeLibrary.GetAttribute<PolymorphicAttribute>(GetType()).PolymorphRequired;


    public virtual (Pin[] In, Pin[] Out) InitialPins => ([], []);
	public virtual Vector2 SizeMultiplier => Vector2.One;

// Properties

    public NetList<Pin> InputPins { get; set; } = new();
    public NetList<Pin> OutputPins { get; set; } = new();

    public NetList<object> InputValues { get; set; } = new();
    public NetList<object> OutputValues { get; set; } = new();

    public NetList<WeakReference<Wire>> InputWires { get; set; } = new();
    public NetList<NetList<WeakReference<Wire>>> OutputWires { get; set; } = new(); // Performance: NetHashSet instead..? (when and if it will exist)

    public NetDictionary<Type, Meta> Meta { get; set; } = new();
    
    public bool Dirty { get; set; } = false;
    

// Constructor

    public Node()
    {
        PinWireChangeEvent += CallPolymorph;

        InitialPins.In.ForEach(InputPins.Add);
        InitialPins.Out.ForEach(OutputPins.Add);
        UpdateArrays();
        Evaluate(); // ?
    }
    
    public void UpdateArrays() {
        InputWires.Resize(InputPins.Count, (_) => new(null));
        InputValues.Resize(InputPins.Count, (index) => {
            var type = TypeLibrary.GetType(InputPins[index].Type);
            return type.IsValueType ? type.Create<object>() : null;
        });

        OutputWires.Resize(OutputPins.Count, (_) => new());
        OutputValues.Resize(OutputPins.Count, (index) => {
            var type = TypeLibrary.GetType(OutputPins[index].Type);
            return type.IsValueType ? type.Create<object>() : null;
        });
    }

// Virtual Methods

    public virtual void Evaluate() { }

    public virtual Node Polymorph(PinWireChange change) => null;

    public virtual void Render(Panel panel) { }

	public virtual Node Clone() {
        var type = GetType();
        var typeDescription = TypeLibrary.GetType(type);
        if (typeDescription.IsGenericType) {
            var generics = TypeLibrary.GetGenericArguments(type);
            return typeDescription.CreateGeneric<Node>(generics);
        }

        return typeDescription.Create<Node>();
    }



// Methods

    public void AddPin(PinType pinType, Pin pin) {
        throw new NotImplementedException();
        // ResizeArrays();
    }
    
    public void RenamePin(PinType pinType, int index, string name) {
        var pins = GetPins(pinType);
        var pin = pins[index];
        pin.Name = name;
        pins[index] = pin; // bruh
    }

    public NetList<Pin> GetPins(PinType pinType) => pinType == PinType.Input ? InputPins : OutputPins;
    public Pin GetPin(PinType pinType, int index) => GetPins(pinType)[index];

    public NetList<object> GetPinValues(PinType pinType) => pinType == PinType.Input ? InputValues : OutputValues;
    public object GetPinValue(PinType pinType, int index) => GetPinValues(pinType)[index];
    
    public T GetInput<T>(int index) => InputValues[index] == null ? default : (T)InputValues[index];
    public T GetOutput<T>(int index) => OutputValues[index] == null ? default : (T)OutputValues[index];
    
    public void SetInput<T>(int index, T value) {
        InputValues[index] = value;
        MarkAsDirty();
    }

    public void SetOutput<T>(int index, T value) => OutputValues[index] = value;
    public void SetOutputGeneric(int index, object value) => OutputValues[index] = value;

    public IEnumerable<Wire> GetAllWires(PinType? pinType = null) {
        if (pinType == null || pinType == PinType.Input)
            foreach (var x in GetInputWires())
                yield return x;
        
        if (pinType == null || pinType == PinType.Output) {
            foreach (var x in GetOutputWires())
                yield return x;
        }
    }

    public IEnumerable<Wire> GetInputWires() {
        foreach (var wireRef in InputWires) {
            Assert.NotNull(wireRef);
            if (wireRef.TryGetTarget(out var wire))
                yield return wire;
        }
    }
    
    public IEnumerable<Wire> GetOutputWires() {
        foreach (var wireList in OutputWires) {
            Assert.NotNull(wireList);
            foreach (var wireRef in wireList) {
                Assert.NotNull(wireRef);
                if (wireRef.TryGetTarget(out var wire))
                    yield return wire;
            }
        }
    }
    
    public IEnumerable<Wire> GetPinWires(PinType pinType, int index) {
        if (pinType == PinType.Output) {
            Assert.NotNull(OutputWires[index]);
            foreach (var wireRef in OutputWires[index]) {
                Assert.NotNull(wireRef);
                if (wireRef.TryGetTarget(out var wire))
                    yield return wire;
            }
        } else {
            Assert.NotNull(InputWires[index]);
            if (InputWires[index].TryGetTarget(out var wire))
                yield return wire;
        }
    }

    internal void SetOutputWire(int index, Wire wire) {
        OutputWires[index].Add(new WeakReference<Wire>(wire));
        PinWireChangeEvent?.Invoke(new PinWireChange(this, PinType.Output, index, wire));
    }

    internal void SetInputWire(int index, Wire wire) {
        InputWires[index] = new WeakReference<Wire>(wire);
        PinWireChangeEvent?.Invoke(new PinWireChange(this, PinType.Input, index, wire));
    }

    internal void UnsetOutputWire(int index, Wire wire) {
        var list = OutputWires[index];
        var immutableList = list.ToImmutableList();
        var foundIndex = immutableList.FindIndex(x => {
            if (!x.TryGetTarget(out var w)) {
                // Collect bad indices?
                return false;
            }
            
            return w == wire;
        });

        if (foundIndex >= 0) {
            list.RemoveAt(foundIndex);
        }

        PinWireChangeEvent?.Invoke(new PinWireChange(this, PinType.Output, index, null));
    }

    internal void UnsetInputWire(int index) {
        InputWires[index].SetTarget(null);
        PinWireChangeEvent?.Invoke(new PinWireChange(this, PinType.Input, index, null));
    }

    private void CallPolymorph(PinWireChange change) {
        var newNode = Polymorph(change);
        if (newNode == null) {
            if (IsPolymorphRequired && change.HasPolymorphType) {
                PolymorphEvent?.Invoke(null);
            }
            
            return;
        }

        newNode.PinWireChangeEvent -= newNode.CallPolymorph;

        GetAllWires(PinType.Output).ForEach(wire => {
            wire.From = newNode;
            newNode.SetOutputWire(wire.FromIndex, wire);
        });

        GetAllWires(PinType.Input).ForEach(wire => {
            wire.To = newNode;
            newNode.SetInputWire(wire.ToIndex, wire);
        });

        PolymorphEvent?.Invoke(newNode);

        newNode.PinWireChangeEvent += newNode.CallPolymorph;
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

    public void MarkAsDirty() => Dirty = true;

	public override string ToString() => GetType().GetDisplayName();

    private bool disposed = false;
    public void Dispose() {
        if (!disposed) {
            GetAllWires().ForEach(x => x.Dispose());

            disposed = true;
        }

        GC.SuppressFinalize(this);
    }

    ~Node() {
        Dispose();
    }
}

