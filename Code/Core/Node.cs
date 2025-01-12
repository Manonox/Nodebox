using System.Collections.Immutable;

namespace Nodebox;


public class Node : IMeta
{
    public virtual bool Tick => false;

    public virtual string Name => "Node";
    public virtual string Desc => "Lorum ipsum...";
    public virtual string[] Groups => Array.Empty<string>();
    public virtual (Pin[] In, Pin[] Out) InitialPins => (Array.Empty<Pin>(), Array.Empty<Pin>());

    internal Pin[] _inputPins = Array.Empty<Pin>();
    internal Pin[] _outputPins = Array.Empty<Pin>();
    internal object[] _inputValues = Array.Empty<object>();
    internal object[] _outputValues = Array.Empty<object>();
    //internal bool[] _outputDirtyMarkers = Array.Empty<bool>();
    internal WeakReference<Wire>[] _inputWires = Array.Empty<WeakReference<Wire>>();
    internal HashSet<WeakReference<Wire>>[] _outputWires = Array.Empty<HashSet<WeakReference<Wire>>>();
    internal Dictionary<Type, Meta> _meta = new();

    public ReadOnlyCollection<Pin> InputPins => _inputPins.AsReadOnly();
    public ReadOnlyCollection<Pin> OutputPins => _outputPins.AsReadOnly();
    public ReadOnlyCollection<object> InputValues => _inputValues.AsReadOnly();
    public ReadOnlyCollection<object> OutputValues => _outputValues.AsReadOnly();
    public ReadOnlyCollection<WeakReference<Wire>> InputConnections => _inputWires.AsReadOnly();
    public IReadOnlyList<IReadOnlyList<WeakReference<Wire>>> OutputConnections => _outputWires.Select(x => x.ToList()).ToList();

    public IEnumerable<Wire> ValidOutputConnections { get {
        foreach (var set in _outputWires) {
            foreach (var x in set) {
                if (x.TryGetTarget(out var wire))
                    yield return wire;
            }
        }
    } }
    

    private void ResizeArrays() {
        Array.Resize(ref _inputValues, _inputPins.Length);
        Array.Resize(ref _inputWires, _inputPins.Length);

        Array.Resize(ref _outputValues, _outputPins.Length);
        var oldOutputPinsLength = _outputWires.Length;
        Array.Resize(ref _outputWires, _outputPins.Length);
        for (int i = oldOutputPinsLength; i < _outputPins.Length; i++)
            _outputWires[i] = new();
    }

    public Node()
    {
        _inputPins = InitialPins.In;
        _outputPins = InitialPins.Out;
        ResizeArrays();
    }


    public void AddOutputPin(Type T, int index) {
        throw new NotImplementedException();
        // ResizeArrays();
    }

    public void AddInputPin(Type T, int index) {
        throw new NotImplementedException();
        // ResizeArrays();
    }
    
    public void RenameInputPin(int index, string name) {
        _inputPins[index].Name = name;
    }

    public void RenameOutputPin(int index, string name) {
        _outputPins[index].Name = name;
    }


    public T GetMeta<T>() {
        _meta.TryGetValue(typeof(T), out Meta value);
        return ((Meta<T>)value).Value;
    }

    public void SetMeta<T>(T value) {
        _meta.Add(typeof(T), new Meta<T>(value));
    }

    public T GetInput<T>(int index) => InputValues[index] == null ? default : (T)InputValues[index];
    public T GetOutput<T>(int index) => OutputValues[index] == null ? default : (T)OutputValues[index];

    internal void SetInput<T>(int index, T value) {
        _inputValues[index] = value;
    }

    protected void SetOutput<T>(int index, T value) {
        _outputValues[index] = value;
    }

    public virtual void Evaluate() { }

    public virtual void Render(GameObject go, Panel panel) { }
}
