namespace Nodebox;
public static class PolymorphHelpers {
    public static (Pin[] In, Pin[] Out) SubstitutePolymorphic<T>(this (Pin[] In, Pin[] Out) pins) => InitialPinsWithType(pins, typeof(T));
    // Do not use, use the staticly..? defined version above
    private static (Pin[] In, Pin[] Out) InitialPinsWithType(this (Pin[] In, Pin[] Out) pins, Type type) {
        pins.In.Enumerate().ForEach(x => {
            if (x.Item.Type != typeof(Polymorphic)) return;
            pins.In[x.Index] = x.Item.WithType(type);
        });

        pins.Out.Enumerate().ForEach(x => {
            if (x.Item.Type != typeof(Polymorphic)) return;
            pins.Out[x.Index] = x.Item.WithType(type);
        });

        return pins;
    }

    public static Node ToType(Type target, Type type, object[] args = null) {
        Assert.NotNull(target);
        Assert.NotNull(type);

        var typeDescription = TypeLibrary.GetType(target);
        if (!typeDescription.IsGenericType) {
            return typeDescription.Create<Node>(args);
        }
        Type[] genericArguments = [ .. typeDescription.GenericArguments.Select(x => type) ];
        return typeDescription.CreateGeneric<Node>(genericArguments, args);
    }
    
    public static Node ToNonGeneric<T>(object[] args = null) =>
        TypeLibrary.GetType<T>().Create<Node>(args);

    public static Node ToNonGenericIfAllInputsDisconnected<T>(PinWireChange change, object[] args = null) {
        Assert.False(TypeLibrary.GetType<T>().IsGenericType);
        if (change.Target.GetInputWires().Any())
            return null;
        return ToNonGeneric<T>();
    }
    
    public static Node ToNonGenericIfAllDisconnected<T>(PinWireChange change, object[] args = null) {
        Assert.False(TypeLibrary.GetType<T>().IsGenericType);
        if (change.Target.GetAllWires().Any())
            return null;
        return ToNonGeneric<T>();
    }

    public static Node ToConnectedType(Type target, PinWireChange change, object[] args = null) {
        Assert.NotNull(change.PolymorphTargetType);
        return ToType(target, change.PolymorphTargetType, args);
    }

    public static Node ToConnectedInputTypeIfRegistered(Type target, PinWireChange change) {
        if (change.PinType == PinType.Output) return null;
        return ToConnectedTypeIfRegistered(target, change);
    }

    public static Node ToConnectedTypeIfRegistered(Type target, PinWireChange change) {
        if (change.PolymorphTargetType == null || !change.IsPinPolymorphic) {
            return null;
        }
        
        static Type GetGenericTypeDefinition(Type type) => TypeLibrary.GetType(type).TargetType;

        var entries = Library.Entries
            .Where(entry => GetGenericTypeDefinition(entry.Type) == target)
            .Where(entry => entry.Generics.Length > 0)
            .Where(entry => entry.Generics[0] == change.PolymorphTargetType);

        if (!entries.Any()) {
            return null;
        }

        return ToConnectedType(target, change);
    }

    public static Node ToConnectedTypeIfInCollection<T>(Type target, PinWireChange change) => ToConnectedTypeIfInCollection(typeof(T), target, change);
    public static Node ToConnectedTypeIfInCollection(Type collection, Type target, PinWireChange change) {
        if (change.PolymorphTargetType == null || !change.IsPinPolymorphic) {
            return null;
        }
        
        var types = Library.GetTypeCollection(collection);
        if (!types.Contains(change.PolymorphTargetType)) {
            return null;
        }

        return ToConnectedType(target, change);
    }
}