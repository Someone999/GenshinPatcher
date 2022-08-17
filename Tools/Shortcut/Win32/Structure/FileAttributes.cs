namespace GenshinPatcher.Tools.Shortcut.Win32.Structure;

[Flags]
public enum FileAttributes
{
    ReadOnly = 1 << 0,
    Hidden = 1 << 1,
    System = 1 << 2,
    Reserved1 = 1 << 3,
    Directory = 1 << 4,
    Archive = 1 << 5,
    Reserved2 = 1 << 6,
    Normal = 1 << 7,
    Temporary = 1 << 8,
    SparseFile = 1 << 9,
    ReparsePoint = 1 << 10,
    Compressed = 1 << 11,
    Offline = 1 << 12,
    NotContentIndexed = 1 << 13,
    Encrypted = 1 << 14
}