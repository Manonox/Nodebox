using Sandbox.Diagnostics;
using Sandbox.Internal;
namespace Nodebox;

public static class TypeLibraryExtensions {
    public static bool SetProperty<T, TValue>(this TypeLibrary typeLibrary, T target, PropertyDescription property, TValue value) {
        Assert.True(typeLibrary.GetType<T>() == property.TypeDescription);
        Assert.True(property.CanWrite);
        Assert.True(property.PropertyType == typeof(TValue));
        return typeLibrary.SetProperty(target, property.Name, value);
    }
}
