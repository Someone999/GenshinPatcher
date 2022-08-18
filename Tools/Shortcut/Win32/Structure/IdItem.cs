namespace GenshinPatcher.Tools.Shortcut.Win32.Structure;

public class IdItem
{
    public short Size { get; }
    public byte[] Data { get; }
    public IdItem(short size, byte[] data)
    {
        Size = size;
        Data = data;
    }
}