namespace Nodebox;


public class Node : IMeta //, ICloneable
{
    public virtual Type[] Generics => [];

// "Class Properties"

    public virtual bool Tick => false;

    public virtual string Name => "Node";
    public virtual string Desc => "Lorum ipsum...";
    public virtual string Icon => "";
    public virtual string[] Groups => Array.Empty<string>();
    public virtual string[] Aliases => Array.Empty<string>();
    public virtual Vector2 SizeMultiplier => Vector2.One;
    public virtual (Pin[] In, Pin[] Out) InitialPins => (Array.Empty<Pin>(), Array.Empty<Pin>());


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
        Dirty = true;
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

    
    public T GetMeta<T>() {
        Meta.TryGetValue(typeof(T), out Meta meta);
        return ((Meta<T>)meta).Value;
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

    public virtual void Evaluate() { }

    public virtual void Render(GameObject go, Panel panel) { }

	public virtual Node Clone() {
        var typeDescription = TypeLibrary.GetType(GetType());
        if (typeDescription.IsGenericType) {
            return typeDescription.CreateGeneric<Node>(Generics);
        }

        return typeDescription.Create<Node>();
    }
}

