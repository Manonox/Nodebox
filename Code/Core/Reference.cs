namespace Nodebox;


public class Reference : IEquatable<Reference> {
    public Reference() {
        Target = null;
        PropertyName = null;
    }

    public Reference(object target, PropertyDescription propertyDescription) {
        Target = target;
        PropertyName = propertyDescription.Name;
    }
    
    public Reference(object target, string propertyName) {
        Target = target;
        PropertyName = propertyName;
    }

    private object _target;
    private string _propertyName;
    private PropertyDescription _propertyDescription;

    public object Target {
		get => _target; set {
        _target = value;
        UpdatePropertyDescription();
    } }

    public string PropertyName {
		get => _propertyName; set {
        _propertyName = value;
        UpdatePropertyDescription();
    } }

    public PropertyDescription PropertyDescription => _propertyDescription;
    public Type PropertyType => PropertyDescription.PropertyType;


    public bool CanWrite => PropertyDescription.CanWrite;
    public bool CanRead => PropertyDescription.CanRead;

    public bool Write(object value) => Write<object>(value);
    public bool Write<T>(T value) {
        if (!IsValid()) return false;
        if (!CanWrite) return false;
        PropertyDescription.SetValue(Target, value);
        return true;
    }

    public bool TryRead(out object value) => TryRead<object>(out value);
    public bool TryRead<T>(out T value) {
        value = default;
        if (!IsValid()) return false;
        if (!CanRead) return false;
        value = Read<T>();
        return true;
    }

    public object Read() => PropertyDescription.GetValue(Target);
    public T Read<T>() => (T)PropertyDescription.GetValue(Target);

    public virtual bool IsValid() {
        return Target != null &&
            PropertyDescription != null &&
            PropertyDescription.TypeDescription.TargetType == Target.GetType();
    }

    private void UpdatePropertyDescription() {
        _propertyDescription = TypeLibrary.GetType(_target.GetType()).GetProperty(_propertyName);
    }

	public override bool Equals(object obj) {
        if (obj is Reference reference) {
            return Equals(reference);
        }
        
        return false;
	}

	public override int GetHashCode() {
        return HashCode.Combine(Target, PropertyDescription);
	}

	public override string ToString() => $"{PropertyDescription?.Name} on {Target?.ToString() ?? "null"}";

	public bool Equals(Reference other) => Target == other.Target && PropertyDescription.Name == other.PropertyDescription.Name;
}

public class Reference<T> : Reference {
    public Reference() {
        Target = null;
        PropertyName = null;
    }

    public Reference(object target, PropertyDescription propertyDescription) : base(target, propertyDescription) { }
    public Reference(object target, string propertyName) : base(target, propertyName) { }
    
    public Type Type { get; private set; } = typeof(T);

	public bool Write(T value) => Write<T>(value);

    public bool TryRead(out T value) => TryRead<T>(out value);

    public new T Read() => Read<T>();
}
