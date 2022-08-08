using System.Text;

namespace GenshinPatcher.Tools;

public static class HashTools
{
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