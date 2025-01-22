
namespace Nodebox.Nodes;


[Register(typeof(Library.Vector))]
[Description("Switches around component positions in a given vector")]
[Tag("Math", "Vector")]
[Polymorphic(true)]
public class Swizzle : Node
{
	public override Vector2 SizeMultiplier => new(0.66f, 1f);
    
	public override (Pin[] In, Pin[] Out) InitialPins => (
        new Pin[] {
            Pin.New<Polymorphic>("V"),
        },
        
        []
    );

	public override Node Polymorph(PinWireChange change) =>
        PolymorphHelpers.ToConnectedTypeIfRegistered(typeof(Swizzle<>), change);
}


[Register(typeof(Library.Vector))]
[Description("Switches around component positions in a given vector")]
[Tag("Math", "Vector")]
[Polymorphic(typeof(Swizzle))]
public class Swizzle<T> : Swizzle
{
    const string LETTERS = "XYZW";
    private static Type VectorBaseType => typeof(T).GetVectorBaseType();
    private static int VectorTypeDimensions => typeof(T).GetVectorTypeDimensions();
    private int? _cachedPermutationCount;
    private int[][] _cachedPermutations;

	public override (Pin[] In, Pin[] Out) InitialPins { get {
        if (!typeof(T).IsVectorType())
            throw new NotImplementedException();

        var inputName = Enumerable.Range(0, VectorTypeDimensions)
            .Select(x => LETTERS[x].ToString())
            .Aggregate((a, b) => a + b);
        
        _cachedPermutationCount ??= GetPermutationCount() - 1;
        if (_cachedPermutations == null) {
            _cachedPermutations ??= new int[_cachedPermutationCount.Value][];
            for (int i = 0; i < _cachedPermutationCount; i++)
                _cachedPermutations[i] = GetPermutation(i + 1);
        }
        
        var outputPins = Enumerable.Range(0, _cachedPermutationCount.Value)
            .Select(x => _cachedPermutations[x])
            .Select(x => x.Select(i => LETTERS[i].ToString()))
            .Select(x => x.Aggregate((a, b) => a + b))
            .Select(name => Pin.New<T>(name)).ToArray();

        return (
            new Pin[] {
                Pin.New<T>(inputName),
            },
            
            outputPins
        );
    } }
    
	public override Node Polymorph(PinWireChange change) =>
        PolymorphHelpers.ToNonGenericIfAllDisconnected<Swizzle>(change);

    private static int GetPermutationCount(int? n = null) {
        n ??= VectorTypeDimensions;
        if (n <= 1) return 1;
        return GetPermutationCount(n - 1) * n.Value;
    }

    private static int[] GetPermutation(int i) {
        int n = VectorTypeDimensions;
        int j = 0;
        int k = 0;
        int[] fact = new int[n];
        int[] perm = new int[n];
        fact[k] = 1;
        while (++k < n)
            fact[k] = fact[k - 1] * k;
        for (k = 0; k < n; ++k) {
            perm[k] = i / fact[n - 1 - k];
            i %= fact[n - 1 - k];
        }
        for (k = n - 1; k > 0; --k)
            for (j = k - 1; j >= 0; --j)
                if (perm[j] <= perm[k])
                    perm[k]++;
        return perm;
    }

    public override void Evaluate() {
        if (VectorBaseType == typeof(float)) {
            float[] input = null;

            if (typeof(T) == typeof(Vector2)) {
                input = [.. GetInput<Vector2>(0).Unpack()];
            }

            if (typeof(T) == typeof(Vector3)) {
                input = [.. GetInput<Vector3>(0).Unpack()];
            }

            if (typeof(T) == typeof(Vector4)) {
                input = [.. GetInput<Vector4>(0).Unpack()];
            }
            
            var type = TypeLibrary.GetType<T>();
            _cachedPermutations.Enumerate().ForEach(x => {
                SetOutputGeneric(x.Index, type.Create<object>( [.. x.Item.Select(i => input[i])] ));
                //SetOutputGeneric(x.Index, x.Item.Select(i => input[i]).PackVectorAny()); // TODO: Measure performance
            });
            return;
        }
        
        if (VectorBaseType == typeof(int)) {
            int[] input = null;

            if (typeof(T) == typeof(Vector2Int)) {
                input = [.. GetInput<Vector2Int>(0).Unpack()];
            }

            if (typeof(T) == typeof(Vector3Int)) {
                input = [.. GetInput<Vector3Int>(0).Unpack()];
            }
            
            var type = TypeLibrary.GetType<T>();
            _cachedPermutations.Enumerate().ForEach(x => {
                SetOutputGeneric(x.Index, type.Create<object>( [.. x.Item.Select(i => input[i])] ));
                //SetOutputGeneric(x.Index, x.Item.Select(i => input[i]).PackVectorAny()); // TODO: Measure performance
            });
            return;
        }

        throw new NotImplementedException();
    }
}
