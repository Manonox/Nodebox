namespace Nodebox.Nodes;

[Register(typeof(Library.Vector))]
[Register(typeof(Angles))]
[Register(typeof(Rotation))]
[Register(typeof(Color))]
[Description("Takes a vector and returns it's components")]
[Tag("Packing")]
public class Unpack<T> : Node
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
        var outputPins = Enumerable.Range(0, VectorTypeDimensions)
            .Select(index => new Pin(VectorBaseType, letters[index].ToString())).ToArray();

        return (
            new Pin[] {
                Pin.New<T>("V"),
            },

            outputPins
        );
    } }

    public override void Evaluate() {
        if (!typeof(T).IsVectorType())
            throw new NotImplementedException();

        if (VectorBaseType == typeof(float)) {
            IEnumerable<float> input = null;
            if (typeof(T) == typeof(Vector2)) {
                input = GetInput<Vector2>(0).Unpack();
            }
            
            if (typeof(T) == typeof(Vector3)) {
                input = GetInput<Vector3>(0).Unpack();
            }

            if (typeof(T) == typeof(Vector4)) {
                input = GetInput<Vector4>(0).Unpack();
            }
        
            if (typeof(T) == typeof(Angles)) {
                input = GetInput<Angles>(0).Unpack();
            }
            
            if (typeof(T) == typeof(Rotation)) {
                input = GetInput<Rotation>(0).Unpack();
            }

            if (typeof(T) == typeof(Color)) {
                input = GetInput<Color>(0).Unpack();
            }
            
            if (input == null)
                throw new NotImplementedException();
            
            input
                .Enumerate()
                .ForEach(x => SetOutput(x.Index, x.Item));
            return;
        }

        if (VectorBaseType == typeof(int)) {
            IEnumerable<int> input = null;
            if (typeof(T) == typeof(Vector2Int)) {
                input = GetInput<Vector2Int>(0).Unpack();
                return;
            }

            if (typeof(T) == typeof(Vector3Int)) {
                input = GetInput<Vector3Int>(0).Unpack();
                return;
            }
            
            if (input == null)
                throw new NotImplementedException();
            
            input
                .Enumerate()
                .ForEach(x => SetOutput(x.Index, x.Item));
            return;
        }

        throw new NotImplementedException();
    }
}
