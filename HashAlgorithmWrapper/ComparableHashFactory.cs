using System.Reflection;
using GenshinPatcher.Reflection;

namespace GenshinPatcher.HashAlgorithmWrapper;

public static class ComparableHashFactory
{

    private static void Init()
    {
        var types = ReflectionAssemblyManager.GetInstance().GetTypes().GetSubTypesOf<IComparableHash>();
        foreach (var type in types)
        {
            ConstructorInfo? constructorInfo = type.GetConstructor(Type.EmptyTypes);
            IComparableHash? ins = (IComparableHash?)constructorInfo?.Invoke(Array.Empty<object>());
            if (ins == null)
            {
                continue;
            }
            
            ComparableHashes.Add(type, ins);
        }
    }

    private static readonly Dictionary<Type, IComparableHash> ComparableHashes = new Dictionary<Type, IComparableHash>();
    public static IComparableHash? GetComparableHash<T>() where T: IComparableHash
    {
        if (ComparableHashes.Count == 0)
        {
            Init();
        }
        return ComparableHashes.ContainsKey(typeof(T))
            ? ComparableHashes[typeof(T)]
            : null;
    }

    public static IComparableHash Default { get; } = new Crc32ComparableHash();
}