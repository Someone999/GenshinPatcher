using System.Runtime.InteropServices;

namespace GenshinPatcher.Tools.Shortcut.Win32.Structure;

[StructLayout(LayoutKind.Explicit, Size = 76)]
public struct ShellLinkHeader
{
    [FieldOffset(0)]
    public int HeaderSize;
    [FieldOffset(4)]
    public long LinkClassIdLow;
    [FieldOffset(12)]
    public long LinkClassIdHigh;
    [FieldOffset(20)]
    public LinkFlags LinkFlags;
    [FieldOffset(24)]
    public FileAttributes FileAttributes;
    [FieldOffset(28)]
    public long CreationTime;
    [FieldOffset(36)]
    public long AccessTime;
    [FieldOffset(44)]
    public long WriteTime;
    [FieldOffset(52)]
    public int FileSize;
    [FieldOffset(56)]
    public int IconIndex;
    [FieldOffset(60)]
    public int ShowCommand;
    [FieldOffset(64)]
    public short HotKey;
    [FieldOffset(66)]
    public short Reserved1;
    [FieldOffset(68)]
    public int Reserved2;
    [FieldOffset(72)]
    public int Reversed3;
}