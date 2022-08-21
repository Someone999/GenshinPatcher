using GenshinPatcher.Tools;

namespace GenshinPatcher.HashAlgorithmWrapper;

public class Sha1HashComputeResult : IHashComputeResult<byte[]>
{

    object IHashComputeResult.Result => Result;
    public byte[] Result { get; }
    public string ConvertToString()
    {
        return Result.GetHashString();
    }
    public bool HashEquals(IHashComputeResult hashComputeResult)
    {
        if (hashComputeResult is Sha1HashComputeResult result)
        {
            return HashEquals(result.ConvertToString());
        }
        
        return false;
    }
    public bool HashEquals(string hash)
    {
        return hash == ConvertToString().ToLower();
    }

    public Sha1HashComputeResult(byte[] result)
    {
        Result = result;
    }
}