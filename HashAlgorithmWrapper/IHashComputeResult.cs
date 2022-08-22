namespace GenshinPatcher.HashAlgorithmWrapper;

/// <summary>
/// 包装后的hash计算结果
/// </summary>
public interface IHashComputeResult
{
    object Result { get; }
    string ConvertToString();
    bool HashEquals(IHashComputeResult hashComputeResult);
    bool HashEquals(string hash);
}

/// <summary>
/// 包装后的hash计算结果
/// </summary>
/// <typeparam name="T">hash结果的类型</typeparam>
public interface IHashComputeResult<out T> : IHashComputeResult
{
    new T Result { get; }
}