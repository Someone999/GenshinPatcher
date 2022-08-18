using System.Runtime.InteropServices;

namespace GenshinPatcher.Tools.Shortcut.Win32.Structure;

public struct DarwinDataBlock
{
    public int BlockSize;
    public int BlockSignature;
    
    [MarshalAs(UnmanagedType.LPArray, SizeConst = 260)]
    public byte[] DarwinDataAnsi;
    
    [MarshalAs(UnmanagedType.LPArray, SizeConst = 520)]
    public byte[] DarwinDataUnicode;
    
}