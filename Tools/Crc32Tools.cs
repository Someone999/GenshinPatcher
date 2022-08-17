using Force.Crc32;

namespace GenshinPatcher.Tools;

public static class Crc32Tools
{
    public static bool IsCrc32CheckPass(byte[] data, uint exceptedCrc32)
    {
        return exceptedCrc32 == CalcCrc32(data) || exceptedCrc32 == CalcCrc32C(data);
    }

    public static uint CalcCrc32(byte[] data)
    {
        return Crc32Algorithm.Compute(data);
    }
    
    public static uint CalcCrc32C(byte[] data)
    {
        return Crc32CAlgorithm.Compute(data);
    }
}