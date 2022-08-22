using GenshinPatcher.Tools.Shortcut.Win32.Structure;

namespace GenshinPatcher.Tools.Shortcut;

public class ShortcutInfo
{
    public ShellLinkHeader LinkHeader { get; } = new ShellLinkHeader();
    public LinkTargetIdList LinkTargetIdList { get; } = new LinkTargetIdList();
    public LinkInfo LinkInfo { get; } = new LinkInfo();
    public StringData StringData { get; } = new StringData();
    public ExtraData ExtraData { get; } = new ExtraData();
}