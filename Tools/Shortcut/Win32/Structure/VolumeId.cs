namespace GenshinPatcher.Tools.Shortcut.Win32.Structure;

public struct VolumeId
{
    public int VolumeSize;
    public int DriveType;
    public int DriveSerialNumber;
    public int VolumeLabelOffset;
    public int VolumeLabelOffsetUnicode;
    public byte[] Data;
}