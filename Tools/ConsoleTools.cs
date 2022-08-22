using System.Runtime.InteropServices;

namespace GenshinPatcher.Tools;

public static class ConsoleTools
{
    [DllImport("msvcrt")]
    internal static extern int system(string command);

    /// <summary>
    /// 显示&quot;按任意键继续...&quot;并等待输入
    /// </summary>
    public static void Pause() => system("pause");
}