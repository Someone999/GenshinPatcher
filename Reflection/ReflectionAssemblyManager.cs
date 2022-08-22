using System.Reflection;

namespace GenshinPatcher.Reflection;


/// <summary>
/// 管理需要用于反射的程序集
/// </summary>
public class ReflectionAssemblyManager
{
    private List<Assembly> _registeredAsm = new List<Assembly>();
    private static ReflectionAssemblyManager? _ins;
    private static readonly object StaticLocker = new object();
    public Assembly[] RegisteredAssemblies => _registeredAsm.ToArray();
    
    /// <summary>
    /// 添加程序集
    /// </summary>
    /// <param name="assembly">要添加的程序集</param>
    public void AddAssembly(Assembly assembly)
    {
        if (_registeredAsm.Contains(assembly))
        {
            return;
        }
        
        _registeredAsm.Add(assembly);
    }

    /// <summary>
    /// 移除程序集
    /// </summary>
    /// <param name="assembly">要移除的程序集</param>
    public void RemoveAssembly(Assembly assembly) => _registeredAsm.Remove(assembly);
    
    
    public static ReflectionAssemblyManager GetInstance()
    {
        if (_ins != null)
        {
            return _ins;
        }
        lock (StaticLocker)
        {
            _ins ??= new ReflectionAssemblyManager();
        }
        
        return _ins;
    }
    
    /// <summary>
    /// 获取当前添加了的程序集中所有的类型
    /// </summary>
    /// <returns>类型集合</returns>

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