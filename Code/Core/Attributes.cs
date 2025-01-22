namespace Nodebox.Attributes;


[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class RegisterAttribute : Attribute {
    public Type[] Array { get; set; } = [];
    public RegisterAttribute() { }
    public RegisterAttribute(params Type[] types) {
        Array = types;
    }
    public RegisterAttribute(Type type, int n) {
        Array = [ .. Enumerable.Range(0, n).Select(_ => type) ];
    }
}


[AttributeUsage(AttributeTargets.Class)]
public class InitializedAttribute : Attribute { }


[AttributeUsage(AttributeTargets.Class)]
public class ReaderAttribute : Attribute { }

[AttributeUsage(AttributeTargets.Class)]
public class WriterAttribute : Attribute { }

[AttributeUsage(AttributeTargets.Class)]
public class PolymorphicAttribute: Attribute {
    public PolymorphicAttribute(bool polymorphRequired = false) {
        PolymorphRequired = polymorphRequired;
    }

    public PolymorphicAttribute(Type parent) {
        Parent = parent;
    }

    public bool PolymorphRequired { get; set; } = false;
    public Type Parent { get; set; } = null;
}
