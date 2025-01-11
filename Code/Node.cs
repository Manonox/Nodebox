namespace Nodebox;

public class Node
{
    public delegate void EvaluatedEventHandler(Node node);
    public event EvaluatedEventHandler OnEvaluated;

    public delegate void PinsChangedEventHandler(Pin[] pins);
    public event PinsChangedEventHandler OnInputPinsChanged;
    public event PinsChangedEventHandler OnOutputPinsChanged;


    public abstract class Pin
    {
        public string Name { get; set; }
        public string Desc { get; set; }
        public Type Type { get; set; }
    }

    public class Pin<T> : Pin
    {   
        public Pin(string name, string desc = null)
        {
            Name = name;
            Desc = desc;
            Type = typeof(T);
        }
    }
    
    public virtual bool Discrete => false;
    public virtual bool Tick => false;

    public virtual string Name => "Node";
    public virtual string Desc => "Lorum ipsum...";
    public virtual string[] Groups => new string[] {};

    private Pin[] _inputPins = new Pin[0];
    private Pin[] _outputPins = new Pin[0];
    private object[] _inputValues = new object[0];
    private object[] _outputValues = new object[0];
    private bool[] _outputDirtyMarkers = new bool[0];
    private (Node Node, int Index)?[] _outputConnections = new (Node, int)?[0];
    
    public Pin[] InputPins { get => _inputPins; set {
        _inputPins = value;
        Array.Resize(ref _inputValues, value.Length);
		OnInputPinsChanged?.Invoke( _inputPins );
	} }

    public Pin[] OutputPins { get => _outputPins; set {
        _outputPins = value;
        Array.Resize(ref _outputValues, value.Length);
        Array.Resize(ref _outputDirtyMarkers, value.Length);
        Array.Resize(ref _outputConnections, value.Length);
        OnOutputPinsChanged?.Invoke(_outputPins);
    } }
    public object[] InputValues { get => _inputValues; set => _inputValues = value; }
    public object[] OutputValues { get => _outputValues; set => _outputValues = value; }
    public bool[] OutputDirtyMarkers { get => _outputDirtyMarkers; set => _outputDirtyMarkers = value; }
    public (Node Node, int Index)?[] OutputConnections { get => _outputConnections; set => _outputConnections = value; }


    public T GetInput<T>(int index, T @default = default)
    {
        if (index < 0 || index >= InputValues.Length)
            throw new IndexOutOfRangeException("Input index is out of range.");

        if (InputValues[index] == null) return @default;
        return (T)InputValues[index];
    }

    public void SetOutput<T>(int index, T value)
    {
        if (index < 0 || index >= OutputValues.Length)
            throw new IndexOutOfRangeException("Output index is out of range.");

        OutputValues[index] = value;
        OutputDirtyMarkers[index] = true;
    }

    public T GetOutput<T>(int index)
    {
        if (index < 0 || index >= OutputValues.Length)
            throw new IndexOutOfRangeException("Output index is out of range.");

        return (T)OutputValues[index];
    }

    public virtual void Eval() { }

    public void Evaluate() {
        OutputDirtyMarkers = new bool[OutputValues.Length];
        Eval();

        var DirtyNodes = new List<Node>();
        for (int i = 0; i < OutputValues.Length; i++)
        {
            if (OutputDirtyMarkers[i])
            {
                var connection = OutputConnections[i];
                if (!connection.HasValue) continue;
                var (node, index) = connection.Value;
                node.InputValues[index] = OutputValues[index];
                DirtyNodes.Add(node);
            }
        }
        
        if (OnEvaluated != null)
            OnEvaluated(this);

        foreach (var node in DirtyNodes) {
            node.Evaluate();
        }
    }

    public void Connect(int outputIndex, Node node, int inputIndex) {
        OutputConnections[outputIndex] = (node, inputIndex);
        node.InputValues[inputIndex] = OutputValues[outputIndex];
        node.Evaluate();
    }

    public void Disconnect(int outputIndex) {
        if (!OutputConnections[outputIndex].HasValue) return;
        var (node, inputIndex) = OutputConnections[outputIndex].Value;
        node.InputValues[inputIndex] = null;
        OutputConnections[outputIndex] = null;
    }

    public virtual void Render(GameObject go, Panel panel) { }
}
