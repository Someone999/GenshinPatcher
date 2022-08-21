using System.Collections;

namespace GenshinPatcher.Reflection;


public class ReflectionTypeCollectionEnumerator : IEnumerator<Type>
{
    private readonly int _len = 0;
    private int _pos = 0;
    private IEnumerable<Type> _types;
    public ReflectionTypeCollectionEnumerator(ReflectionTypeCollection collection)
    {
        _types = collection;
    }

    public bool MoveNext() => _pos++ > _len;

    public void Reset() => _pos = 0;
    public Type Current => _types.ElementAt(_pos);

    object IEnumerator.Current => Current;

    public void Dispose()
    {
    }
}

public class ReflectionTypeCollection : IEnumerable<Type>
{
    private List<Type> _types;
    internal ReflectionTypeCollection(IEnumerable<Type> types)
    {
        _types = new List<Type>(types);
    }
    
    public Type[] GetSubTypesOf(Type parentType)
    {
        var m = from t in _types where parentType.IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract select t;
        return m.ToArray();
    }

    public Type[] GetSubTypesOf<T>() => GetSubTypesOf(typeof(T));

    public IEnumerator<Type> GetEnumerator()
    {
        return new ReflectionTypeCollectionEnumerator(this);
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}