using System.Security.Cryptography;
using GenshinPatcher.GameFile;
using GenshinPatcher.HashAlgorithmWrapper;
using GenshinPatchTools.Game;
using Newtonsoft.Json;

namespace GenshinPatcher.Tools;

public static class PatchFileTools
{
    static GameUpdateFiles? InternalGetUpdateFiles(out Exception? exception)
    {
        try
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("UseAgent", UserAgent.Chrome);
            string filesUrl = Configs.Config?["updateInfoUrl"].GetValue<string>() ?? "https://2982029868.oss-cn-shanghai.aliyuncs.com/index.json";
            var filesJson = client.GetStringAsync(filesUrl).Result;
            exception = null;
            return JsonConvert.DeserializeObject<GameUpdateFiles>(filesJson);
        }
        catch (Exception e)
        {
            exception = e;
            return null;
        }
        
    }

    public static GameUpdateFiles GetUpdateFiles(int maxRetry = 3)
    {
        int retryCount = 0;
        var ret = InternalGetUpdateFiles(out var exception);
        while (ret == null && exception != null)
        {
            if (exception is not AggregateException aggregateException)
            {
                return new GameUpdateFiles();
            }

            if (aggregateException.InnerExceptions[0] is HttpRequestException && retryCount++ < maxRetry)
            {
                ret = InternalGetUpdateFiles(out exception);
            }
        }

        return ret ?? new GameUpdateFiles();
    }
    
    public static bool HasFileOfClientType(ClientType clientType, GameUpdateFiles? gameUpdateFiles = null)
    {
        return (gameUpdateFiles ?? GetUpdateFiles())?.Files.Any(file => file.ClientType == clientType) ?? false;
    }
    
    public static bool HasFileOfVersion(string version, GameUpdateFiles? gameUpdateFiles = null)
    {
        return (gameUpdateFiles ?? GetUpdateFiles())?.Files.Any(file => file.Version == version) ?? false;
    }

    public static bool IsFileHashCheckPass(IGamePatchFileInfo patchFileInfo, byte[] downloadedBytes, IComparableHash comparableHash)
    {
        string downloadedHash = comparableHash.Compute(downloadedBytes).ConvertToString();
        return downloadedHash == patchFileInfo.Hash;
    }
}