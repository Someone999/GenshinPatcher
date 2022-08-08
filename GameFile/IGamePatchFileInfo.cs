using GenshinPatchTools.Game;

namespace GenshinPatcher.GameFile;

public interface IGamePatchFileInfo
{
    string Version { get; set; }
    string DownloadUrl { get; set; }
    string Hash { get; set; }
    ClientType ClientType { get; set; }
}