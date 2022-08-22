using System.Security.Cryptography;
using GenshinPatcher.GameFile;
using GenshinPatcher.HashAlgorithmWrapper;
using GenshinPatchTools.Game;
using GenshinPatchTools.Game.Patch;
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

    /// <summary>
    /// 从指定的源下载json并解析成GameUpdateFiles
    /// </summary>
    /// <param name="maxRetry">最大重试数量</param>
    /// <returns></returns>
    public static GameUpdateFiles GetUpdateFiles(int maxRetry = 3)
    {
        int retryCount = 0;
        var ret = InternalGetUpdateFiles(out var exception);
        while (ret == null && exception != null && maxRetry > 0)
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
    
    /// <summary>
    /// 判断指定的GameUpdateFiles中有无相应客户端类型的文件
    /// </summary>
    /// <param name="clientType">指定客户端类型</param>
    /// <param name="gameUpdateFiles">指定的GameUpdateFiles，留空会调用GetUpdateFiles获取</param>
    /// <returns>有无相应的客户端类型的文件</returns>
    public static bool HasFileOfClientType(ClientType clientType, GameUpdateFiles? gameUpdateFiles = null)
    {
        return (gameUpdateFiles ?? GetUpdateFiles())?.Files.Any(file => file.ClientType == clientType) ?? false;
    }
    
    /// <summary>
    /// 判断指定的GameUpdateFiles中有无相应版本的文件
    /// </summary>
    /// <param name="version">指定的版本</param>
    /// <param name="gameUpdateFiles">指定的GameUpdateFiles，留空会调用GetUpdateFiles获取</param>
    /// <returns>有无相应的服务器版本的文件</returns>
    
    public static bool HasFileOfVersion(string version, GameUpdateFiles? gameUpdateFiles = null)
    {
        return (gameUpdateFiles ?? GetUpdateFiles())?.Files.Any(file => file.Version == version) ?? false;
    }
    
    /// <summary>
    /// 下载完的文件的hash是否和指定的hash相等
    /// </summary>
    /// <param name="patchFileInfo">预期的文件信息</param>
    /// <param name="downloadedBytes">下载完的字节</param>
    /// <param name="comparableHash">hash算法</param>
    /// <returns></returns>

    public static bool IsFileHashCheckPass(IGamePatchFileInfo patchFileInfo, byte[] downloadedBytes, IComparableHash comparableHash)
    {
        string downloadedHash = comparableHash.Compute(downloadedBytes).ConvertToString();
        return downloadedHash == patchFileInfo.Hash;
    }

    /// <summary>
    /// 在指定的目录遍历文件并且计算hash值并录入json文件
    /// </summary>
    /// <param name="patchFolder">补丁文件目录</param>
    /// <param name="outputFile">要输出到的json文件</param>
    public static void GenerateJsonFile(string patchFolder, string outputFile)
    {
        FileScanner fileScanner = new FileScanner();
        fileScanner.Scan(patchFolder);
        GameUpdateFiles files = new GameUpdateFiles();
        List<GamePatchFileInfo> updateFiles = new List<GamePatchFileInfo>();
        var hashAlgorithm = ComparableHashFactory.Default;

        var rslt = fileScanner.Files.ToList();
        foreach (var scannedFileInfo in rslt)
        {
            string[] split = scannedFileInfo.RelatePath.Split('\\').Skip(1).ToArray();
            string serverType = split[0];
            string gameVersion = split[1];
            string path = string.Join('/', split.Skip(2).ToArray());
            GamePatchFileInfo fileInfo = new GamePatchFileInfo
            {
                Version = gameVersion,
                ClientType = Enum.Parse<ClientType>(serverType),
                DownloadUrl = path,
                Sha1 = hashAlgorithm.Compute(File.ReadAllBytes(scannedFileInfo.FileInfo.FullName)).ConvertToString()
            };
            updateFiles.Add(fileInfo);
            files.Files = updateFiles.ToArray();
        }
        string json = JsonConvert.SerializeObject(files, Formatting.Indented);
        File.WriteAllText(outputFile, json);
    }
}