namespace Nodebox.Nodes;

[Register(typeof(Library.Vector))]
[Register(typeof(Angles))]
[Register(typeof(Rotation))]
[Register(typeof(Color))]
[Description("Takes components of a vector type and returns the resulting vector")]
[Tag("Packing")]
public class Pack<T> : Node
{
	public override Vector2 SizeMultiplier => new(0.66f, 1f);


    const string LETTERS = "XYZW";
    const string COLOR_LETTERS = "RGBA";
    private static Type VectorBaseType => typeof(T).GetVectorBaseType();
    private static int VectorTypeDimensions => typeof(T).GetVectorTypeDimensions();

	public override (Pin[] In, Pin[] Out) InitialPins { get {
        if (!typeof(T).IsVectorType())
            throw new NotImplementedException();

        var letters = typeof(T) != typeof(Color) ? LETTERS : COLOR_LETTERS;
        var inputPins = Enumerable.Range(0, VectorTypeDimensions)
            .Select(index => new Pin(VectorBaseType, letters[index].ToString())).ToArray();

        return (
            inputPins,
            
            new Pin[] {
                Pin.New<T>("V"),
            }
        );
    } }

    public override void Evaluate() {
        if (!typeof(T).IsVectorType())
            throw new NotImplementedException();

        if (VectorBaseType == typeof(float)) {
            var inputPins = Enumerable.Range(0, VectorTypeDimensions)
                .Select(index => (object)GetInput<float>(index));
            SetOutputGeneric(0, TypeLibrary.GetType<T>().Create<object>( [.. inputPins] ));
            return;
        }
        
        if (VectorBaseType == typeof(int)) {
            var inputPins = Enumerable.Range(0, VectorTypeDimensions)
                .Select(index => (object)GetInput<int>(index));
            SetOutputGeneric(0, TypeLibrary.GetType<T>().Create<object>( [.. inputPins] ));
            return;
        }

        throw new NotImplementedException();
    }
}
