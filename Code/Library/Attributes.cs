namespace Nodebox.Attributes;


public abstract class Dummy { }


[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class RegisterNode : Attribute { }


[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public class Generics(Type[] array) : Attribute {
    public Type[] Array { get; set; } = array;
    public Generics(Type type) : this(new Type[] { type }) { }
}


[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class Hidden : Attribute { }

