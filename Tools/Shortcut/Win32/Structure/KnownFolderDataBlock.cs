namespace GenshinPatcher.Tools.Shortcut.Win32.Structure;

public struct KnownFolderDataBlock
{
    public int BlockSize;
    public int BlockSignature;
    public long KnownFolderIdLow;
    public long KnownFolderIdHigh;
    public int Offset;
}