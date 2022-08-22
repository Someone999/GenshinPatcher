using System.Security.Cryptography;
using Force.Crc32;

namespace GenshinPatcher.HashAlgorithmWrapper;

/// <summary>
/// 包装了Crc32算法的
/// </summary>
public class Crc32ComparableHash : IComparableHash<uint>
{
    public HashAlgorithm HashAlgorithm { get; } = new Crc32Algorithm();
    public IHashComputeResult<uint> Compute(byte[] data)
    {
        return new Crc32HashComputeResult(BitConverter.ToUInt32(HashAlgorithm.ComputeHash(data).Reverse().ToArray()));
    }
    IHashComputeResult IComparableHash.Compute(byte[] data)
    {
        return Compute(data);
    }
}