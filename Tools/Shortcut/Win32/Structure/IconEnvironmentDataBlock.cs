using System.Runtime.InteropServices;

namespace GenshinPatcher.Tools.Shortcut.Win32.Structure;

public struct IconEnvironmentDataBlock
{
    public int BlockSize;
    public int BlockSignature;
    
    [MarshalAs(UnmanagedType.LPArray, SizeConst = 260)]
    public byte[] TargetAnsi;
    
    [MarshalAs(UnmanagedType.LPArray, SizeConst = 520)]
    public byte[] TargetUnicode;
}