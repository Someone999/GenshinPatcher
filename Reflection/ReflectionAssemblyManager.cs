using System.Reflection;

namespace GenshinPatcher.Reflection;

public class ReflectionAssemblyManager
{
    private List<Assembly> _registeredAsm = new List<Assembly>();
    private static volatile ReflectionAssemblyManager? _ins;
    private static readonly object StaticLocker = new object();
    public Assembly[] RegisteredAssemblies => _registeredAsm.ToArray();
    public void AddAssembly(Assembly assembly)
    {
        if (_registeredAsm.Contains(assembly))
        {
            return;
        }
        
        _registeredAsm.Add(assembly);
    }

    public void RemoveAssembly(Assembly assembly) => _registeredAsm.Remove(assembly);
    
    public static ReflectionAssemblyManager GetInstance()
    {
        if (_ins != null)
        {
            return _ins;
        }
        lock (StaticLocker)
        {
            if (_ins != null)
            {
                return _ins;
            }
            
            lock (StaticLocker)
            {
                _ins = new ReflectionAssemblyManager();
            }
        }
        
        return _ins;
    }

    public ReflectionTypeCollection GetTypes()
    {
        List<Type> types = new List<Type>();
        var allTypes = from asm in _registeredAsm select asm.GetTypes();
        foreach (var allType in allTypes)
        {
            types.AddRange(allType);
        }
        return new ReflectionTypeCollection(types);
    }

    

}