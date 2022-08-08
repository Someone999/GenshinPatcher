using System.Diagnostics;
using System.Reflection;
using System.Security.Principal;

namespace GenshinPatcher.Tools;

public static class ProcessTools
{
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