namespace GenshinPatcher.Tools.Shortcut.Win32.Structure;

public struct TrackerDataBlock
{
    public int BlockSize;
    public int BlockSignature;
    public int Length;
    public int Version;
    public long MachineIdLow;
    public long MachineIdHigh;
    public long DroidHighLow;
    public long DroidHighHigh;
    public long DroidLowLow;
    public long DroidLowHigh;
    
    public long DroidBirthHighLow;
    public long DroidBirthHighHigh;
    public long DroidBirthLowLow;
    public long DroidBirthLowHigh;
}