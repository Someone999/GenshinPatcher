namespace GenshinPatcher.Tools.Shortcut.Win32.Structure;

public struct SpecialFolderDataBlock
{
    public int BlockSize;
    public int BlockSignature;
    public int SpecialFolderId;
    public int Offset;
}