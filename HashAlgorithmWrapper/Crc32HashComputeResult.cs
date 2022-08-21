namespace GenshinPatcher.HashAlgorithmWrapper;

public class Crc32HashComputeResult : IHashComputeResult<uint>
{
    object IHashComputeResult.Result => Result;

    public uint Result { get; }
    public string ConvertToString() => Result.ToString("x2");
   
    public bool HashEquals(IHashComputeResult hashComputeResult)
    {
        if (hashComputeResult is Crc32HashComputeResult crc32HashComputeResult)
        {
            return crc32HashComputeResult.ConvertToString().ToLower() == ConvertToString();
        }
        
        return false;
    }
    
    public bool HashEquals(string hash)
    {
        return ConvertToString() == hash;
    }

    public Crc32HashComputeResult(uint result)
    {
        Result = result;
    }
}