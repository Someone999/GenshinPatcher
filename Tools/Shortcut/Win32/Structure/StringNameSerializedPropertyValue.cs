namespace GenshinPatcher.Tools.Shortcut.Win32.Structure;

public struct StringNameSerializedPropertyValue
{
    public int ValueSize;
    public int NameSize;
    public byte Reserved;
    public string Name;
}