namespace GenshinPatcher.Tools.Shortcut.Win32.Structure;

public class SerializedPropertyStore
{
    public int StoreSize;
    public List<SerializedPropertyStorage> Storage = new List<SerializedPropertyStorage>();
}