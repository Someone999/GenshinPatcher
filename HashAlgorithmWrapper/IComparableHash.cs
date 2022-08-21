using System.Security.Cryptography;

namespace GenshinPatcher.HashAlgorithmWrapper;

public interface IComparableHash
{
    HashAlgorithm HashAlgorithm { get; }
    IHashComputeResult Compute(byte[] data);
}

public interface IComparableHash<out T> : IComparableHash
{
    new IHashComputeResult<T> Compute(byte[] data);
}

public interface IHashComputeResult<out T> : IHashComputeResult
{
    new T Result { get; }
}