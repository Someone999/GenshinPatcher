using GenshinPatchTools.Game;
using Newtonsoft.Json;

namespace GenshinPatcher.GameFile;

public class GamePatchFileInfo : IGamePatchFileInfo
{
    [JsonProperty("version")]
    public string Version { get; set; } = "";

    [JsonProperty("downloadUrl")]
    public string DownloadUrl { get; set; } = "";

    public string Hash => Sha1;

    [JsonProperty("sha1")]
    public string Sha1 { get; set; } = "";
    
    [JsonProperty("clientType")]
    public ClientType ClientType { get; set; }

    [JsonProperty("downloadPath")]
    public string DownloadPath { get; set; } = "";
}