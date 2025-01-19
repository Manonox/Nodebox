namespace Nodebox;

using T = Dummy;

[Title("Nodebox Library")]
public class Library: Component {

    public List<Type> AllTypes = new(){
    // Base
        typeof(float),
        typeof(double),
        typeof(int),
        //typeof(long),
        typeof(bool),
        
        typeof(Vector2),
        typeof(Vector3),
        typeof(Vector4),
        typeof(Vector2Int),
        typeof(Vector3Int),

        typeof(Angles),
        typeof(Rotation),
        typeof(Color),
        
        typeof(Reference),
        typeof(GameObject),
        // ...

    // Generic
        typeof(Reference<T>),
        // typeof(List<T>),
        // typeof(Dictionary<T, T>),
    };

    public struct Entry(Type type, IReadOnlyList<Type> generics): IComparable<Entry> {
        public readonly string Name { get {
            var typeDesc = TypeLibrary.GetType(Type);
            var title = typeDesc.Title.Replace(" ", null);
            if (!typeDesc.IsGenericType) {
                return title;
            }

            var generics = string.Join(", ", Generics.Select(x => x.GetPrettyName()));
            return $"{title}<{generics}>";
        }}

        public Type Type { get; private set; } = type;
        public IReadOnlyList<Type> Generics { get; private set; } = generics;
        
        // public static List<Entry> FromRow((Type Type, List<List<Type>> Generics) row) {
        //     var typeDescription = TypeLibrary.GetType(row.Type);
        //     if (typeDescription.GetAttribute<Hidden>() != null) return [];

        //     if (typeDescription.IsGenericType) {
        //         return [.. row.Generics.Select(x => new Entry(row.Type, x))];
        //     }

        //     return new() { new Entry(row.Type, []) };
        // }

		public readonly int CompareTo(Entry other) {
            var nameComparison = Name.CompareTo(other.Name);
            if (nameComparison != 0) return nameComparison;
            // ..?
            return 0;
        }

		[Pure]
        [System.Diagnostics.Contracts.Pure]
        public readonly Node CreateNode() {
            var typeDescription = TypeLibrary.GetType(Type);
            if (!typeDescription.IsGenericType) {
                return typeDescription.Create<Node>();
            } else {
                return typeDescription.CreateGeneric<Node>( [.. Generics] );
            }
        }
    }

    public delegate void LibraryPopulateEventHandler(Library library);
    public static event LibraryPopulateEventHandler OnLibraryPopulate;
    [Property] public bool ShouldLoadDefaults { get; set; } = true;
    public List<Entry> Entries { get; set; } = new();

    public Library() {
        if (ShouldLoadDefaults)
            LoadDefaults();
        OnLibraryPopulate?.Invoke(this);
        Entries.Sort();
    }

    private void LoadDefaults() {
        static IEnumerable<List<Type>> GetCombinations(List<Type> types, int count) {
            if (count == 1) return types.Select(t => new List<Type> { t });
            return GetCombinations(types, count - 1)
                .SelectMany(t => types, (t1, t2) => t1.Concat(new List<Type> { t2 }).ToList());
        }

        TypeLibrary.GetTypesWithAttribute<RegisterNode>(false)
            .Select(x => x.Type)
            .Where(type => !type.HasAttribute<Hidden>())
            .ForEach(type => {
                if (!type.IsGenericType) {
                    Entries.Add(new(type.TargetType, []));
                    return;
                }

                var presetGenerics = type.GetAttributes<Generics>();
                if (presetGenerics.Any()) {
                    presetGenerics.ForEach(x => {
                        Entries.Add(new(type.TargetType, x.Array));
                    });
                    return;
                }

                var genericCount = type.GenericArguments.Length;
                foreach (var combination in GetCombinations(AllTypes, genericCount)) {
                    Entries.Add(new(type.TargetType, combination));
                }
            });
    }
}
