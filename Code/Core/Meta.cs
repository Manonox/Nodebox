namespace Nodebox;


internal class Meta( Type type )
{
    public Type Type { get; set; } = type;
}

internal class Meta<T>( T value ) : Meta(typeof(T))
{
	public T Value { get; set; } = value;
}


public interface IMeta {
    public abstract T GetMeta<T>();
    public abstract void SetMeta<T>(T value);
}
