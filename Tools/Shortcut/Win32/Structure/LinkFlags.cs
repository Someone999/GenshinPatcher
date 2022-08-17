namespace GenshinPatcher.Tools.Shortcut.Win32.Structure;

[Flags]
public enum LinkFlags
{
    HasLinkInfo = 1 << 0,
    HasName = 1 << 1,
    HasRelativePath = 1 << 2,
    HasWorkingDir = 1 << 3,
    HasArguments = 1 << 4,
    HasIconLocation = 1 << 5,
    IsUnicode = 1 << 6,
    ForceNoLinkInfo = 1 << 7,
    HasExpString = 1 << 8,
    RunInSeparateProcess = 1 << 9,
    Unused1 = 1 << 10,
    HasDarwinId = 1 << 11,
    RunAsUser = 1 << 12,
    HasExpIcon = 1 << 13,
    NoProcessIdlAlias = 1 << 14,
    Unused2 = 1 << 15,
    RunWithShimLayer = 1 << 16,
    ForceNoLinkTrack = 1 << 17,
    EnableTargetMetadata = 1 << 18,
    DisableLinkPathTracking = 1 << 19,
    DisableKnownFolderTracking = 1 << 20,
    DisableKnownFolderAlias = 1 << 21,
    AllowLinkToLink = 1 << 22,
    UnAliasOnSave = 1 << 23,
    PreferEnvironmentPath = 1 << 24,
    KeepLocalIdListForUNCTarget = 1 << 25
}