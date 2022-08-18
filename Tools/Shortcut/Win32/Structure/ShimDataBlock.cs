namespace GenshinPatcher.Tools.Shortcut.Win32.Structure;

public struct ShimDataBlock
{
    public int BlockSize;
    public int BlockSignature;
    public byte[] LayerName;
}