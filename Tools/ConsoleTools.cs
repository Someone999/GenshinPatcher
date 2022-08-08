using System.Runtime.InteropServices;

namespace GenshinPatcher.Tools;

public static class ConsoleTools
{
    [DllImport("msvcrt")]
    internal static extern int system(string command);

    public static void Pause() => system("pause");
}