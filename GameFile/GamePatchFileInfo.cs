using GenshinPatchTools.Game;
using Newtonsoft.Json;

namespace GenshinPatcher.GameFile;

public class GamePatchFileInfo : IGamePatchFileInfo
{
    [JsonProperty("version")]
    public string Version { get; set; } = "";

    [JsonProperty("downloadUrl")]
    public string DownloadUrl { get; set; } = "";

    [JsonProperty("hash")]
    public string Hash { get; set; } = "";
    
    [JsonProperty("clientType")]
    public ClientType ClientType { get; set; }
}