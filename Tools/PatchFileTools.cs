using System.Security.Cryptography;
using GenshinPatcher.GameFile;
using GenshinPatchTools.Game;
using Newtonsoft.Json;

namespace GenshinPatcher.Tools;

public static class PatchFileTools
{
    public static GameUpdateFiles? GetUpdateFiles()
    {
        HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Add("UseAgent", UserAgent.Chrome);
        string filesUrl = "https://lxzqaq.xyz/index.json";
        string filesJson = client.GetStringAsync(filesUrl).Result;
        
        return JsonConvert.DeserializeObject<GameUpdateFiles>(filesJson);
    }
    
    public static bool HasFileOfClientType(ClientType clientType, GameUpdateFiles? gameUpdateFiles = null)
    {
        return (gameUpdateFiles ?? GetUpdateFiles())?.Files.Any(file => file.ClientType == clientType) ?? false;
    }

    public static bool Verify(IGamePatchFileInfo patchFileInfo, byte[] downloadedBytes, HashAlgorithm algorithm)
    {
        string downloadedHash = algorithm.ComputeHash(downloadedBytes).GetHashString();
        return downloadedHash == patchFileInfo.Hash;
    }
}