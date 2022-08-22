using Newtonsoft.Json;

namespace GenshinPatcher.GameFile;

public class GameUpdateFiles
{
    [JsonProperty("latestVersion")]
    public string LatestVersion { get; internal set; } = "";

    [JsonProperty("files")]
    public GamePatchFileInfo[] Files { get; internal set; } = Array.Empty<GamePatchFileInfo>();
}