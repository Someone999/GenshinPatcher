using GenshinPatcher.Tools.Shortcut.Win32.Structure;

namespace GenshinPatcher.Tools.Shortcut;

public class ShortcutInfo
{
    public ShellLinkHeader LinkHeader { get; }
    public LinkTargetIdList LinkTargetIdList { get; }
    public LinkInfo LinkInfo { get; }
    
}


//不要动这个类，这个类要跟着文档写，而且还没写完
public class LinkInfo
{
    public int LinkInfoSize { get; private set; }
    public int LinkInfoHeaderSize { get; private set; }
    public int LinkInfoFlags { get; private set; }
    public int VolumeIdOffset { get; private set; }
    public int LocalBasePathOffset { get; private set; }
    public int CommonNetworkRelativeLinkOffset { get; private set; }
    public int CommonPathSuffixOffset { get; private set; }
}