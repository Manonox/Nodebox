namespace Nodebox.Nodes;

[Register(typeof(Vector2), typeof(float))]
// [Register(typeof(float), typeof(Vector2))]
[Register(typeof(Vector3), typeof(float))]
// [Register(typeof(float), typeof(Vector3))]
[Register(typeof(Vector2Int), typeof(int))]
// [Register(typeof(int), typeof(Vector2Int))]
[Register(typeof(Vector2), typeof(Vector2))]
[Description("Merges two vector types together")]
[Tag("Packing")]
public class Append<TIn0, TIn1> : Node
{
    const string LETTERS = "XYZW";
    private static Type VectorBaseType => typeof(TIn0).GetVectorBaseType();
    private static int In0VectorTypeDimensions => typeof(TIn0).GetVectorTypeDimensions();
    private static int In1VectorTypeDimensions => typeof(TIn1).GetVectorTypeDimensions();
    private static Type OutType { get {
        if (typeof(TIn0) == typeof(Vector2)) {
            if (typeof(TIn1) == typeof(float)) {
                return typeof(Vector3);
            }

            if (typeof(TIn1) == typeof(Vector2)) {
                return typeof(Vector4);
            }
        }

        if (typeof(TIn0) == typeof(Vector3)) {
            if (typeof(TIn1) == typeof(float)) {
                return typeof(Vector4);
            }
        }

        if (typeof(TIn0) == typeof(Vector2Int)) {
            if (typeof(TIn1) == typeof(int)) {
                return typeof(Vector3Int);
            }
        }

        throw new NotImplementedException();
    } }

	public override (Pin[] In, Pin[] Out) InitialPins { get {
        var name0 = Enumerable.Range(0, In0VectorTypeDimensions)
            .Select(x => LETTERS[x].ToString())
            .Aggregate((a, b) => a + b);
        var name1 = Enumerable.Range(In0VectorTypeDimensions, In1VectorTypeDimensions)
            .Select(x => LETTERS[x].ToString())
            .Aggregate((a, b) => a + b);

        if (OutType == null)
            throw new NotImplementedException();

        return (
            new Pin[] {
                Pin.New<TIn0>(name0),
                Pin.New<TIn1>(name1),
            },
            
            new Pin[] {
                new(OutType, "V"),
            }
        );
    } }

    public override void Evaluate() {
        if (typeof(TIn0) == typeof(Vector2) && typeof(TIn1) == typeof(float)) {
            SetOutput(0, GetInput<Vector2>(0).Unpack().Append(GetInput<float>(1)).PackVector3());
            return;
        }

        if (typeof(TIn0) == typeof(Vector3) && typeof(TIn1) == typeof(float)) {
            SetOutput(0, GetInput<Vector3>(0).Unpack().Append(GetInput<float>(1)).PackVector4());
            return;
        }
        
        if (typeof(TIn0) == typeof(Vector2Int) && typeof(TIn1) == typeof(int)) {
            SetOutput(0, GetInput<Vector2Int>(0).Unpack().Append(GetInput<int>(1)).PackVector3Int());
            return;
        }
        
        if (typeof(TIn0) == typeof(Vector2) && typeof(TIn1) == typeof(Vector2)) {
            SetOutput(0, GetInput<Vector2>(0).Unpack().Concat(GetInput<Vector2>(1).Unpack()).PackVector4());
            return;
        }

        throw new NotImplementedException();
    }
}

