namespace GenshinPatcher.Tools.Shortcut.Win32.Structure;

public class LinkInfo
{
    public int LinkInfoSize { get; private set; }
    public int LinkInfoHeaderSize { get; private set; }
    public LinkInfoFlags LinkInfoFlags { get; private set; }
    public int VolumeIdOffset { get; private set; }
    public int LocalBasePathOffset { get; private set; }
    public int CommonNetworkRelativeLinkOffset { get; private set; }
    public int CommonPathSuffixOffset { get; private set; }
    public int LocalBasePathOffsetUnicode { get; private set; }
    public int CommonPathSuffixOffsetUnicode { get; private set; }
}