using System.Text;

namespace GenshinPatcher.Tools;

public static class HashTools
{
    /// <summary>
    /// 将字节数组型hash转换成每个字节16进制拼接的字符串
    /// </summary>
    /// <param name="hashBytes">字符数组</param>
    /// <returns>每个字节16进制拼接成的字符串</returns>
    public static string GetHashString(this byte[] hashBytes)
    {
        StringBuilder builder = new StringBuilder();
        foreach (var hashByte in hashBytes)
        {
            builder.Append($"{hashByte:x2}");
        }
        return builder.ToString();
    }
}