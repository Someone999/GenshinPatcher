namespace GenshinPatcher.Tools.Shortcut.Win32.Structure;

public struct CommonNetworkRelativeLink
{
    public int CommonNetworkRelativeLinkSize;
    public int CommonNetworkRelativeLinkFlags;
    public int NetNameOffset;
    public int DeviceNameOffset;
    public int NetworkProviderType;
    public int NetNameOffsetUnicode;
    public string? NetName;
    public string? DeviceName;
    public string? NetNameUnicode;
    public string? DeviceNameUnicode;
}