// See https://aka.ms/new-console-template for more information

using System.IO.Compression;
using System.Security.Cryptography;
using System.Security.Principal;
using GenshinPatcher.Download;
using GenshinPatcher.GameFile;
using GenshinPatcher.Tools;
using GenshinPatchTools.Game;
using GenshinPatchTools.Game.Patch;

namespace GenshinPatcher;

class Program
{
    static void Main(string[] args)
    {

        var f = PatchFileTools.GetUpdateFiles();
        return;
        var identity = WindowsIdentity.GetCurrent();
        var principal = new WindowsPrincipal(identity);
        
        
        
        if (!principal.IsInRole(WindowsBuiltInRole.Administrator))
        {
            Console.WriteLine("请以管理员身份运行此程序");
            Console.Read();
            return;
        }
        
        Console.WriteLine("请将游戏路径复制到控制台窗口");
        string? filePath = Console.ReadLine()?.Trim('\"');
        if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
        {
            Console.WriteLine("文件不存在");
            Console.Read();
            return;
        }
        string fileName = Path.GetFileName(filePath);
        if (string.IsNullOrEmpty(fileName) || (fileName != "GenshinImpact.exe" && fileName != "YuanShen.exe"))
        {
            Console.WriteLine("这个文件不是游戏文件");
            Console.Read();
            return;
        }

        string[] tips = {"输入一个数字:", "1.为游戏打补丁", "2.还原游戏文件" };
        string[] validOptions = {"1", "2"};
        foreach (var tip in tips)
        {
            Console.WriteLine(tip);
        }

        string? idx = Console.ReadLine();
        while (string.IsNullOrEmpty(idx) || validOptions.All(option => option != idx))
        {
            Console.WriteLine("输入无效，请重新输入");
            idx = Console.ReadLine();
        }

        switch (idx)
        {
            case "1":
                Patch(fileName, filePath);
                break;
            case "2":
                UnPatch(fileName, filePath);
                break;
        }
    }

    
    
    

    private static void Patch(string fileName, string filePath)
    {
        while (ProcessTools.IsGameRunning(fileName))
        {
            Console.WriteLine("游戏正在运行，请先退出游戏");
            Console.Read();
        }
        
        GameInfo gameInfo = GameInfo.GetByGameExecutable(filePath);
        Patcher patcher = new Patcher(gameInfo);
        
        if (patcher.CheckPatchFiles().IsFailed())
        {
            var gameUpdateFiles = PatchFileTools.GetUpdateFiles();
            if (!PatchFileTools.HasFileOfClientType(gameInfo.ClientType, gameUpdateFiles))
            {
                Console.WriteLine("找不到所需的补丁文件");
                Console.Read();
                return;
            }

            Downloader downloader = new Downloader();
            MemoryStream memoryStream = new MemoryStream();
            IGamePatchFileInfo? updateFileInfo = gameUpdateFiles?.Files.FirstOrDefault(file => file.ClientType == gameInfo.ClientType);
            if (updateFileInfo == null)
            {
                Console.WriteLine("找不到所需的补丁文件");
                Console.Read();
                return;
            }
            Console.WriteLine("开始下载文件");
            int retryCount = 0;
            downloader.OnDownloadFailed += (url, exception) =>
            {
                retryCount++;
                if (retryCount < downloader.MaxRetry)
                {
                    Console.WriteLine("下载失败，正在重试");
                }
                else
                {
                    Console.WriteLine("下载补丁文件失败");
                    Console.Read();
                    Environment.Exit(0);
                }
            };
            downloader.OnStreamDownloadCompleted += (_, _) => Console.WriteLine("下载完成"); 
            downloader.Download(updateFileInfo.DownloadUrl, memoryStream);
            if (!PatchFileTools.Verify(updateFileInfo, memoryStream.GetBuffer(), SHA1.Create()))
            {
                Console.WriteLine("文件验证失败");
                Console.Read();
                return;
            }
            string downloadedFileName = $"patch_{updateFileInfo.Version}_{updateFileInfo.ClientType}.zip";
            FileStream fileStream = File.Create(downloadedFileName);
            fileStream.Write(memoryStream.GetBuffer());
            memoryStream.Close();
            fileStream.Flush();
            
            ZipProcessor.Extract(new ZipArchive(fileStream, ZipArchiveMode.Read), "./patch/");
            fileStream.Close();
        }
        var patchResult = patcher.Patch();

        switch (patchResult)
        {
            case PatchResult.HasPatched:
                Console.WriteLine("游戏已经打过补丁了");
                Console.Read();
                return;
            case PatchResult.IoError:
                Console.WriteLine("替换文件时出现问题");
                Console.Read();
                return;
            case PatchResult.PermissionDenied:
                Console.WriteLine("没有足够的权限，程序将在3秒后重启");
                Thread.Sleep(3000);
                ProcessTools.Elevate();
                return;
            case PatchResult.GameFileNotFound:
                Console.WriteLine("找不到游戏文件");
                Console.Read();
                return;
            case PatchResult.CanNotBackup:
                Console.WriteLine("备份文件失败");
                Console.Read();
                return;
            case PatchResult.PatchFileNotFound:
                Console.WriteLine("找不到补丁文件");
                Console.Read();
                return;
            case PatchResult.NotPatched:
            case PatchResult.UnknownError:
            case PatchResult.NotRestored:
            case PatchResult.BackupFileNotFound:
            
            case PatchResult.Failed:
            default:
                Console.WriteLine("出现未知错误");
                Console.Read();
                return;
            case PatchResult.Ok:
                Console.WriteLine("完成");
                Console.Read();
                return;
        }
    }
    
    private static void UnPatch(string fileName, string filePath)
    {

        while (ProcessTools.IsGameRunning(fileName))
        {
            Console.WriteLine("游戏正在运行，请先退出游戏");
            Console.Read();
        }
        
        GameInfo gameInfo = GameInfo.GetByGameExecutable(filePath);
        Patcher patcher = new Patcher(gameInfo);
        
        if (patcher.GetPatchState() == PatchResult.NotPatched)
        {
            Console.WriteLine("游戏还没打补丁");
            Console.Read();
            return;
        }
        var patchResult = patcher.UnPatch();
        
        switch (patchResult)
        {
            case PatchResult.NotPatched:
                Console.WriteLine("游戏还没打补丁");
                Console.Read();
                return;
            case PatchResult.NotRestored:
                Console.WriteLine("恢复失败");
                Console.Read();
                return;
            case PatchResult.IoError:
                Console.WriteLine("替换文件时出现问题");
                Console.Read();
                return;
            case PatchResult.PermissionDenied:
                Console.WriteLine("没有足够的权限，程序将在3秒后重启");
                Thread.Sleep(3000);
                ProcessTools.Elevate();
                return;
            case PatchResult.GameFileNotFound:
                Console.WriteLine("找不到游戏文件");
                Console.Read();
                return;
            case PatchResult.BackupFileNotFound:
                Console.WriteLine("找不到备份文件");
                Console.Read();
                return;
            case PatchResult.Ok:
                Console.WriteLine("完成");
                Console.Read();
                return;
            case PatchResult.PatchFileNotFound:
            case PatchResult.UnknownError:
            case PatchResult.CanNotBackup:
            case PatchResult.Failed:
            case PatchResult.HasPatched:
            default:
                Console.WriteLine("出现未知错误");
                Console.Read();
                return;
            
        }
        
    }
}