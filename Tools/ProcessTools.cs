using System.Diagnostics;
using System.Reflection;
using System.Security.Principal;

namespace GenshinPatcher.Tools;

public static class ProcessTools
{
    /// <summary>
    /// 如果运行程序的用户没有在管理员用户组，则提示提权
    /// </summary>
    public static void CheckUserGroup()
    {
        var identity = WindowsIdentity.GetCurrent();
        var principal = new WindowsPrincipal(identity);

        if (principal.IsInRole(WindowsBuiltInRole.Administrator))
        {
            return;
        }
        Console.WriteLine("请以管理员身份运行此程序");
        ConsoleTools.Pause();
        Environment.Exit(0);
    }
    
    /// <summary>
    /// 检测游戏是不是正在运行
    /// </summary>
    /// <param name="fileName">游戏文件名</param>
    /// <returns>判断结果</returns>

    public static bool IsGameRunning(string fileName)
    {
        Process[] processes = Process.GetProcesses();
        foreach(var process in processes)
        {
            try
            {
                ProcessModule? mainModule = process.MainModule;
                if (mainModule == null || mainModule.FileName != Path.GetFileNameWithoutExtension(fileName))
                {
                    continue;
                }

                return true;
            }
            catch (Exception)
            {
                //nothing to do currently
            }
        }
        return false;
    }
}