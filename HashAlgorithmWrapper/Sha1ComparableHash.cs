using System.Security.Cryptography;

namespace GenshinPatcher.HashAlgorithmWrapper;

public class Sha1ComparableHash: IComparableHash<byte[]>
{

    public HashAlgorithm HashAlgorithm { get; } = SHA1.Create();
    public IHashComputeResult<byte[]> Compute(byte[] data)
    {
        return new Sha1HashComputeResult(HashAlgorithm.ComputeHash(data));
    }
    IHashComputeResult IComparableHash.Compute(byte[] data)
    {
        return Compute(data);
    }
}