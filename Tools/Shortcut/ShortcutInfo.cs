using GenshinPatcher.Tools.Shortcut.Win32.Structure;

namespace GenshinPatcher.Tools.Shortcut;

public class ShortcutInfo
{
    public ShellLinkHeader LinkHeader { get; }
    public LinkTargetIdList LinkTargetIdList { get; }
    public LinkInfo LinkInfo { get; }
    public StringData StringData { get; }
    public ExtraData ExtraData { get; }
}