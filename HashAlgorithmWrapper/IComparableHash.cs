using System.Security.Cryptography;

namespace GenshinPatcher.HashAlgorithmWrapper;

/// <summary>
/// 为方便hash比较而创建的接口
/// </summary>
public interface IComparableHash
{
    /// <summary>
    /// 包装的算法
    /// </summary>
    HashAlgorithm HashAlgorithm { get; }
    
    /// <summary>
    /// 计算hash
    /// </summary>
    /// <param name="data">数据</param>
    /// <returns>计算结果</returns>
    IHashComputeResult Compute(byte[] data);
}

/// <summary>
/// <inheritdoc cref="IComparableHash"/>>
/// </summary>
/// <typeparam name="T">hash计算结果的类型</typeparam>
public interface IComparableHash<out T> : IComparableHash
{
    /// <summary>
    /// <inheritdoc cref="IComparableHash.Compute" />
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    new IHashComputeResult<T> Compute(byte[] data);
}

