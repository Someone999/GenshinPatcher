namespace GenshinPatcher.Tools.Shortcut.Win32.Structure;

public struct TypedPropertyValue
{
    public short ValueType;
    public short Padding;
    public byte[] Value;
}