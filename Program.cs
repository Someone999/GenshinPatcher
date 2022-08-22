// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using Force.Crc32;
using GenshinPatcher.Download;
using GenshinPatcher.GameFile;
using GenshinPatcher.HashAlgorithmWrapper;
using GenshinPatcher.Reflection;
using GenshinPatcher.Tools;
using GenshinPatcher.Tools.Shortcut.Win32.Structure;
using GenshinPatchTools.Config;
using GenshinPatchTools.Game;
using GenshinPatchTools.Game.Patch;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GenshinPatcher;

class Program
{
    static void Main(string[] args)
    {
        //不要删掉这些，之后测试要用
        /*unsafe
        {
            var file = File.OpenRead(@"C:\Users\Public\Desktop\腾讯QQ.lnk");
            byte[] bts = new byte[0x4C];
            ShellLinkHeader* shellLinkHeader = (ShellLinkHeader*)Marshal.AllocHGlobal(Marshal.SizeOf<ShellLinkHeader>());
            var readSize = file.Read(bts, 0, 0x4C);
            Marshal.Copy(bts, 0, new IntPtr(shellLinkHeader), 0x4C);
        }*/
        Console.WriteLine("请将游戏文件拖到控制台窗口");
        string? filePath = Console.ReadLine()?.Trim('\"');
        if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
        {
            Console.WriteLine("文件不存在");
            ConsoleTools.Pause();
            return;
        }
        string fileName = Path.GetFileName(filePath);
        if (string.IsNullOrEmpty(fileName) || (fileName != "GenshinImpact.exe" && fileName != "YuanShen.exe"))
        {
            Console.WriteLine("这个文件不是游戏文件");
            ConsoleTools.Pause();
            return;
        }
        string[] tips = {"输入一个数字:", "1.为游戏打补丁", "2.还原游戏文件", "3.获取补丁状态" };
        string[] validOptions = {"1", "2", "3"};
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
            case "3":
                GetPatchState(fileName, filePath);
                break;
        }
    }

    
    
    

    private static void Patch(string fileName, string filePath)
    {
        Console.WriteLine("请输入游戏版本");
        string? version = Console.ReadLine();
        while (string.IsNullOrEmpty(version))
        {
            Console.WriteLine("输入的版本无效");
            version = Console.ReadLine();
        }
        string patchFileFolder = Configs.Config?["patchFileFolder"].GetValue<string>() ?? "./patch/files";
        string patchFileExtractFolder = Configs.Config?["patchExtractFolder"].GetValue<string>() ?? "./patch/files";
        
        
        //测试时不检查是不是管理员和游戏是否正在运行
        /*
        ProcessTools.CheckUserGroup();
        while (ProcessTools.IsGameRunning(fileName))
        {
            Console.WriteLine("游戏正在运行，请先退出游戏");
            ConsoleTools.Pause();
        }*/
        
        GameInfo gameInfo = GameInfo.GetByGameExecutable(filePath);
        Patcher patcher = new Patcher(gameInfo, Path.Combine(patchFileExtractFolder, version));
        
        if (patcher.CheckPatchFiles().IsFailed())
        {
            var gameUpdateFiles = PatchFileTools.GetUpdateFiles();
            if (!PatchFileTools.HasFileOfClientType(gameInfo.ClientType, gameUpdateFiles))
            {
                Console.WriteLine("不支持的客户端类型");
                ConsoleTools.Pause();
                return;
            }
            
            if (!PatchFileTools.HasFileOfVersion(version, gameUpdateFiles))
            {
                Console.WriteLine($"找不到版本{version}的补丁文件");
                ConsoleTools.Pause();
                return;
            }

            Downloader downloader = new Downloader();
            MemoryStream memoryStream = new MemoryStream();
            IGamePatchFileInfo? updateFileInfo = 
                gameUpdateFiles.Files.FirstOrDefault(file => file.ClientType == gameInfo.ClientType && version == file.Version);
            if (updateFileInfo == null)
            {
                Console.WriteLine("找不到所需的补丁文件");
                ConsoleTools.Pause();
                return;
            }
            Console.WriteLine("开始下载文件");
            int retryCount = 0;
            downloader.OnDownloadFailed += (_, _) =>
            {
                retryCount++;
                if (retryCount < downloader.MaxRetry)
                {
                    Console.WriteLine("下载失败，正在重试");
                }
                else
                {
                    Console.WriteLine("下载补丁文件失败");
                    ConsoleTools.Pause();
                    Environment.Exit(0);
                }
            };
            downloader.OnStreamDownloadCompleted += (_, _) => Console.WriteLine("下载完成"); 
            downloader.Download(updateFileInfo.DownloadUrl, memoryStream);
            
            if (!PatchFileTools.IsFileHashCheckPass(updateFileInfo, memoryStream.ToArray(), ComparableHashFactory.Default))
            {
                Console.WriteLine("文件验证失败");
                ConsoleTools.Pause();
                return;
            }
            
            string downloadedFileName = $"patch_{updateFileInfo.Version}_{updateFileInfo.ClientType}.zip";
            
            if (!Directory.Exists(patchFileFolder))
            {
                Directory.CreateDirectory(patchFileFolder);
            }
            FileStream fileStream = File.Create(Path.Combine(patchFileFolder, downloadedFileName));
            
            fileStream.Write(memoryStream.ToArray());
            memoryStream.Close();
            fileStream.Flush();
            
            ZipProcessor.Extract(new ZipArchive(fileStream, ZipArchiveMode.Read), patchFileExtractFolder);
            fileStream.Close();
        }
        var patchResult = patcher.Patch();

        switch (patchResult)
        {
            case PatchResult.HasPatched:
                Console.WriteLine("游戏已经打过补丁了");
                ConsoleTools.Pause();
                return;
            case PatchResult.IoError:
                Console.WriteLine("替换文件时出现问题");
                ConsoleTools.Pause();
                return;
            case PatchResult.PermissionDenied:
                Console.WriteLine("没有足够的权限，请以管理员身份运行此程序，程序将在3秒后退出");
                Thread.Sleep(3000);
                Environment.Exit(0);
                return;
            case PatchResult.GameFileNotFound:
                Console.WriteLine("找不到游戏文件");
                ConsoleTools.Pause();
                return;
            case PatchResult.CanNotBackup:
                Console.WriteLine("备份文件失败");
                ConsoleTools.Pause();
                return;
            case PatchResult.PatchFileNotFound:
                Console.WriteLine("找不到补丁文件");
                ConsoleTools.Pause();
                return;
            case PatchResult.UnknownClientType:
                Console.WriteLine("未知的客户端类型");
                ConsoleTools.Pause();
                return;
            case PatchResult.Ok:
                Console.WriteLine("完成");
                ConsoleTools.Pause();
                return;
            case PatchResult.NotPatched:
            case PatchResult.UnknownError:
            case PatchResult.NotRestored:
            case PatchResult.BackupFileNotFound:
            case PatchResult.Failed:
            
            default:
                Console.WriteLine("出现未知错误");
                ConsoleTools.Pause();
                return;
            
        }
    }
    
    private static void UnPatch(string fileName, string filePath)
    {
        //测试时不检查是不是管理员和游戏是否正在运行
        /*ProcessTools.CheckUserGroup();
        while (ProcessTools.IsGameRunning(fileName))
        {
            Console.WriteLine("游戏正在运行，请先退出游戏");
            ConsoleTools.Pause();
        }*/
        
        GameInfo gameInfo = GameInfo.GetByGameExecutable(filePath);
        Patcher patcher = new Patcher(gameInfo);
        
        if (patcher.GetPatchState() == PatchResult.NotPatched)
        {
            Console.WriteLine("游戏还没打补丁");
            ConsoleTools.Pause();
            return;
        }
        var patchResult = patcher.UnPatch();
        
        switch (patchResult)
        {
            case PatchResult.NotPatched:
                Console.WriteLine("游戏还没打补丁");
                ConsoleTools.Pause();
                return;
            case PatchResult.NotRestored:
                Console.WriteLine("恢复失败");
                ConsoleTools.Pause();
                return;
            case PatchResult.IoError:
                Console.WriteLine("替换文件时出现问题");
                ConsoleTools.Pause();
                return;
            case PatchResult.PermissionDenied:
                Console.WriteLine("没有足够的权限，请以管理员身份运行此程序，程序将在3秒后退出");
                Thread.Sleep(3000);
                Environment.Exit(0);
                return;
            case PatchResult.GameFileNotFound:
                Console.WriteLine("找不到游戏文件");
                ConsoleTools.Pause();
                return;
            case PatchResult.BackupFileNotFound:
                Console.WriteLine("找不到备份文件");
                ConsoleTools.Pause();
                return;
            case PatchResult.UnknownClientType:
                Console.WriteLine("未知的客户端类型");
                ConsoleTools.Pause();
                return;
            case PatchResult.Ok:
                Console.WriteLine("完成");
                ConsoleTools.Pause();
                return;
            case PatchResult.PatchFileNotFound:
            case PatchResult.UnknownError:
            case PatchResult.CanNotBackup:
            case PatchResult.Failed:
            case PatchResult.HasPatched:
            default:
                Console.WriteLine("出现未知错误");
                ConsoleTools.Pause();
                return;
        }
    }
    
    static void GetPatchState(string fileName, string filePath)
    {
        string patchFileExtractFolder = Configs.Config?["patchExtractFolder"].GetValue<string>() ?? "./patch/files";
        var state = new Patcher(GameInfo.GetByGameExecutable(filePath), patchFileExtractFolder).GetPatchState();
        switch (state)
        {
            case PatchResult.HasPatched:
                Console.WriteLine("已经打过补丁了");
                ConsoleTools.Pause();
                return;
            case PatchResult.NotPatched:
                Console.WriteLine("还没打补丁");
                ConsoleTools.Pause();
                return;
            case PatchResult.PermissionDenied:
                ProcessTools.CheckUserGroup();
                return;
            case PatchResult.Ok:
            case PatchResult.NotRestored:
            case PatchResult.UnknownError:
            case PatchResult.IoError:
            case PatchResult.GameFileNotFound:
            case PatchResult.BackupFileNotFound:
            case PatchResult.PatchFileNotFound:
            case PatchResult.CanNotBackup:
            case PatchResult.UnknownClientType:
            case PatchResult.Failed:
            default:
                Console.WriteLine("获取状态失败");
                ConsoleTools.Pause();
                return;
        }
    }
}