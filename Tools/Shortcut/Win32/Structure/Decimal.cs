namespace GenshinPatcher.Tools.Shortcut.Win32.Structure;

public struct Decimal
{
    public short Reversed;
    public byte Scale;
    public byte Sign;
    public int HighBits;
    public long LowBits;
}