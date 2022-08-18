using System.Runtime.InteropServices;

namespace GenshinPatcher.Tools.Shortcut.Win32.Structure;

public struct ConsoleDataBlock
{
    public int BlockSize;
    public int BlockSignature;
    public FillAttributes FillAttributes;
    public FillAttributes PopupFillAttributes;
    public short ScreenBufferSizeX;
    public short ScreenBufferSizeY;
    public short WindowSizeX;
    public short WindowSizeY;
    public short WindowOriginX;
    public short WindowOriginY;
    public int Unused1;
    public int Unused2;
    public int FontSize;
    public FontFamily FontFamily;
    public int FontWeight;
    [MarshalAs(UnmanagedType.LPArray, SizeConst = 64)]
    public byte[] FaceName;

    public int CursorSize;
    public int FullScreen;
    public int QuickEdit;
    public int InsertMode;
    public int AutoPosition;
    public int HistoryBufferSize;
    public int NumberOfHistoryBuffers;
    public int HistoryNoDup;

    [MarshalAs(UnmanagedType.LPArray, SizeConst = 64)]
    public byte[] ColorTable;
}