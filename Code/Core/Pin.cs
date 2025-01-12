namespace Nodebox;

public class Pin
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
}

public class Pin<T> : Pin
{
    public Pin(string name, string desc = null) : base(typeof(T), name, desc) { }
}