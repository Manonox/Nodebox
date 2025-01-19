namespace Nodebox;

public enum PinType {
	Input,
	Output,
}
public static class PinTypeExtensions
{
    public static PinType Opposite(this PinType pinType) => pinType == PinType.Input ? PinType.Output : PinType.Input;
}

public struct Pin
{
    public Type Type { get; set; }
    public string Name { get; set; }
    public string Desc { get; set; }

    public Pin(Type type, string name, string desc = null)
    {
        Type = type;
        Name = name;
        Desc = desc;
    }

    public static Pin New<T>(string name, string desc = null) => new(typeof(T), name, desc);
}
