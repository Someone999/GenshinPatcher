using System.Diagnostics;
using System.Reflection;

namespace GenshinPatcher.Tools;

public static class ProcessTools
{
    public static void Elevate()
    {
        string path = Assembly.GetExecutingAssembly().Location;
        path = path.Substring(0, path.Length - 3);
        path += "exe";
        ProcessStartInfo startInfo = new ProcessStartInfo(path, " -elevated")
        {
            WorkingDirectory = Path.GetDirectoryName(path),
            Verb = "runas",
            CreateNoWindow = false,
            UseShellExecute = false
        };
        Process.Start(startInfo);
        Thread.Sleep(50);
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
                ;//nothing to do currently
            }
        }
        return false;
    }
}