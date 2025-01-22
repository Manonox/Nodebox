namespace Nodebox;


public static class Library {
    public abstract record All { }
    public abstract record Float { }
    public abstract record Integer { }
    public abstract record IntegerUnsigned { }
    public abstract record Vector { }
    public abstract record FloatN { }
    public abstract record IntegerN { }

    public static readonly Dictionary<Type, HashSet<Type>> Types = new(){
        { typeof(bool), new() {
            } },

        { typeof(float), new() {
            typeof(Float),
            typeof(FloatN),
            } },
        
        { typeof(double), new() {
            typeof(Float),
            typeof(FloatN),
            } },

        { typeof(byte), new() {
            typeof(IntegerUnsigned),
            } },
        
        { typeof(int), new() {
            typeof(Integer),
            typeof(IntegerN),
            } },

        { typeof(long), new() {
            typeof(Integer),
            typeof(IntegerN),
            } },

        { typeof(char), new() {
            typeof(IntegerUnsigned),
            } },

        { typeof(string), new() {
            } },

        { typeof(Vector2), new() {
            typeof(FloatN),
            typeof(Vector),
            } },

        { typeof(Vector3), new() {
            typeof(FloatN),
            typeof(Vector),
            } },

        { typeof(Vector4), new() {
            typeof(FloatN),
            typeof(Vector),
            } },

        { typeof(Vector2Int), new() {
            typeof(IntegerN),
            typeof(Vector),
            } },

        { typeof(Vector3Int), new() {
            typeof(IntegerN),
            typeof(Vector),
            } },

        { typeof(Angles), new() {
            } },

        { typeof(Rotation), new() {
            } },

        { typeof(Color), new() {
            } },

        { typeof(GameObject), new() {
            } },
            
        { typeof(Reference), new() {
            } },

        { typeof(Reference<>), new() {
            } },

        { typeof(List<>), new() {
            } },

        { typeof(Dictionary<,>), new() {
            } },
    };

    public static Dictionary<Type, HashSet<Type>> TypeCollections;

    private static Dictionary<Type, HashSet<Type>> GenerateTypeCollections() {
        var result = new Dictionary<Type, HashSet<Type>>();
        var all = result.GetOrCreate(typeof(All));
        Types.ForEach(x => {
            var type = x.Key;
            all.Add(type);

            x.Value.ForEach(collection => {
                var set = result.GetOrCreate(collection);
                set.Add(type);
            });
        });

        return result;
    }

    public static HashSet<Type> AllTypes => GetTypeCollection<All>();
    public static HashSet<Type> GetTypeCollection<T>() => GetTypeCollection(typeof(T));
    public static HashSet<Type> GetTypeCollection(Type type) {
        TypeCollections.TryGetValue(type, out var types);
        return types;
    }

    public static Dictionary<(Type In, Type Out), Func<object, object>> ImplicitConvertions { get; set; }
    private static bool IsGenericType(Type In) => TypeLibrary.GetType(In).IsGenericType;
    public static bool TryGetImplicitConversion(Type In, Type Out, out Func<object, object> func) {
        if (ImplicitConvertions.TryGetValue((In, Out), out func))
            return true;
            
        if (IsGenericType(In)) {
            var InGeneric = TypeLibrary.GetType(In).TargetType; // GetGenericTypeDefinition
            if (ImplicitConvertions.TryGetValue((InGeneric, Out), out func))
                return true;
        }
        return false;
    }

    public struct Entry: IComparable<Entry> {
        [Property] public Type Type { get; private set; }

        public Entry(Type type) {
            Type = type;
        }
        
        public Entry(Type type, IEnumerable<Type> generics) {
            Type = TypeLibrary.GetType(type).MakeGenericType( [.. generics] );
        }

        public Entry(Type type, Type[] generics) {
            Type = TypeLibrary.GetType(type).MakeGenericType(generics);
        }

        public readonly string Name => Type.GetDisplayName();
        public readonly bool IsGenericType => TypeLibrary.GetType(Type).IsGenericType;
        public readonly Type[] Generics { get {
            if (!IsGenericType) {
                return null;
            }

            return TypeLibrary.GetGenericArguments(Type);
        } }
        public readonly bool Polymorphic => TypeLibrary.GetType(Type).HasAttribute<PolymorphicAttribute>(false);
        public readonly Type PolymorphParent => TypeLibrary.GetType(Type).GetAttribute<PolymorphicAttribute>(false).Parent;
        public readonly bool PolymorphRequired => Polymorphic && TypeLibrary.GetType(Type).GetAttribute<PolymorphicAttribute>(false).PolymorphRequired;
        public readonly bool Initialized => TypeLibrary.GetType(Type).HasAttribute<InitializedAttribute>(false);

		public readonly int CompareTo(Entry other) {
            return Type.GetDisplayName().CompareTo(other.Type.GetDisplayName());
        }

		[Pure]
        [System.Diagnostics.Contracts.Pure]
        public readonly Node CreateNode(params object[] args) => Type.CreateClosedGeneric<Node>(args);
    }

    public static List<Entry> Entries { get; set; } = [];

    
    static Library() {
        Reload();
    }

    [ConCmd("nodebox_reload")]
    public static void Reload() {
        ImplicitConvertions = new() {
            { (typeof(float), typeof(int)), x => Convert.ToInt32(x) },
            { (typeof(int), typeof(float)), x => Convert.ToSingle(x) },

            { (typeof(int), typeof(double)), value => Convert.ToDouble(value) },
            { (typeof(double), typeof(int)), value => Convert.ToInt32(value) },

            { (typeof(double), typeof(float)), value => Convert.ToSingle(value) },
            { (typeof(float), typeof(double)), value => Convert.ToDouble(value) },

            //{ (typeof(Reference<>), typeof(Reference)), x => (Reference)x },
            // Vector types?

            { (typeof(Reference<>), typeof(Reference)), x => (Reference)x },
        };

        TypeCollections = GenerateTypeCollections();

        Entries.Clear();
        TypeLibrary.GetTypesWithAttribute<RegisterAttribute>(false)
            .ForEach(x => {
                var typeDescription = x.Type;
                var type = typeDescription.TargetType;
                if (!typeDescription.IsGenericType) {
                    Entries.Add(new(type));
                    return;
                }

                var generics = x.Attribute.Array;
                if (generics.Length == 0) {
                    Log.Error($"Bad RegisterAttribute on {type.GetDisplayName()}");
                    return;
                }

                var collection = generics[0];
                var substitutions = GetTypeCollection(collection);
                if (substitutions == null) {
                    Entries.Add(new(type, generics));
                    return;
                }

                substitutions
                    .Select(sub => generics.Select(type => {
                        if (type == collection) {
                            return sub;
                        }

                        return type;
                    }))
                    .ForEach(subGenerics => {
                        try {
                            Entries.Add(new(type, subGenerics));
                        }
                        catch (Exception e) {
                            var genericsStr = string.Join(", ", subGenerics.Select(x => x.GetDisplayName()));
                            Log.Warning($"Couldn't register {type.GetDisplayName()} with {genericsStr}!");
                            Log.Warning(e);
                        }
                    });
            });
        Entries.Sort();
    }
}
