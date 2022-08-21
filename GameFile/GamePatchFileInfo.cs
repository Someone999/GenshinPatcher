using GenshinPatchTools.Game;
using Newtonsoft.Json;

namespace GenshinPatcher.GameFile;

public class GamePatchFileInfo : IGamePatchFileInfo
{
    [JsonProperty("version")]
    public string Version { get; set; } = "";

    [JsonProperty("downloadUrl")]
    public string DownloadUrl { get; set; } = "";

    public string Hash => Crc32;

    [JsonProperty("crc32")]
    public string Crc32 { get; set; } = "";
    
    [JsonProperty("clientType")]
    public ClientType ClientType { get; set; }
}