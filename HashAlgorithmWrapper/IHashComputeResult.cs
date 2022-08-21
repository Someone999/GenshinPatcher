namespace GenshinPatcher.HashAlgorithmWrapper;

public interface IHashComputeResult
{
    object Result { get; }
    string ConvertToString();
    bool HashEquals(IHashComputeResult hashComputeResult);
    bool HashEquals(string hash);
}