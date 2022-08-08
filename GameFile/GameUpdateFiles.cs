using Newtonsoft.Json;

namespace GenshinPatcher.GameFile;

public class GameUpdateFiles
{
    [JsonProperty("latestVersion")]
    public string LatestVersion { get; private set; } = "";

    [JsonProperty("files")]
    public IGamePatchFileInfo[] Files { get; private set; } = Array.Empty<IGamePatchFileInfo>();
}