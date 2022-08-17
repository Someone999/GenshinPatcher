using GenshinPatcher.Download.Delegates;
using GenshinPatcher.Tools;

namespace GenshinPatcher.Download;


public class Downloader
{
    private static HttpClient _httpClient = new HttpClient();
    public Downloader()
    {
        _httpClient.DefaultRequestHeaders.Add("UserAgent", UserAgent.Chrome);
    }
    public bool RetryWhenFailed { get; set; } = false;
    public int MaxRetry { get; set; } = 3;

    public int BufferSize { get; set; } = 8192;

    public event DownloadCompletedEventHandler? OnDownloadCompleted;
    public event DownloadFailedEventHandler? OnDownloadFailed;
    public event DownloadProgressChangedEventHandler? OnDownloadProgressChanged;
    public event StreamDownloadCompletedEventHandler? OnStreamDownloadCompleted;

    private bool InternalDownload(string url, Stream stream, bool closeOutputStreamWhenFailed)
    {
        var downloadTask = _httpClient.GetAsync(url);
        
        
        
        if (downloadTask.IsFaulted)
        {
            OnDownloadFailed?.Invoke(url, downloadTask.Exception);
            return false;
        }
        
        
        Stream? netFileStream = null;
        try
        {
            
            var response = downloadTask.Result;
            netFileStream = response.Content.ReadAsStream();
            
            long realSize = 0, total = response.Content.Headers.ContentLength ?? long.MaxValue;
            int realBufferSize = total > BufferSize
                ? BufferSize
                : (int)total;
            byte[] buffer = new byte[realBufferSize];
            int readSize;
            do
            {
                readSize = netFileStream.Read(buffer, 0, realBufferSize);
                realSize += readSize;
                stream.Write(buffer, 0, readSize);
                double percent = (double)realSize / total;
                OnDownloadProgressChanged?.Invoke(percent, realSize, total);
            } while (realSize < total && readSize == BufferSize);
            netFileStream.Close();
            stream.Close();
            OnStreamDownloadCompleted?.Invoke(url, stream);
            return true;

        }
        catch (Exception e)
        {
            OnDownloadFailed?.Invoke(url, e);
            stream.Close();
            netFileStream?.Close();

            return false;
        }
        
    }

    private void InternalDownloadToFile(string url, string file, int retryCount, bool? closeOutputStreamWhenFailed)
    {
        int maxRetry = RetryWhenFailed
            ? 0
            : MaxRetry;
        string? localFileDirectory = Path.GetDirectoryName(file);
        localFileDirectory ??= ".";
        if (!Directory.Exists(localFileDirectory))
        {
            Directory.CreateDirectory(localFileDirectory);
        }
        var fileStream = File.Create(file);
        bool suc = InternalDownload(url, fileStream, closeOutputStreamWhenFailed ?? true);
        while (!suc)
        {
            if (retryCount >= maxRetry)
            {
                break;
            }
            fileStream = File.Create(file);
            suc = InternalDownload(url, fileStream, closeOutputStreamWhenFailed ?? retryCount >= maxRetry);
            retryCount++;
        }
    }
    
    private void InternalDownloadToStream(string url, Stream stream, int retryCount, bool? closeOutputStreamWhenFailed)
    {
        int maxRetry = RetryWhenFailed
            ? 0
            : MaxRetry;
        bool suc = InternalDownload(url, stream, closeOutputStreamWhenFailed ?? MaxRetry == 0);
        while (!suc)
        {
            if (retryCount > maxRetry)
            {
                break;
            }
            suc = InternalDownload(url, stream, closeOutputStreamWhenFailed ?? retryCount >= maxRetry);
            retryCount++;
        }
    }

    public void Download(string url, string file, bool? closeOutputStreamWhenFailed = null)
    {
        InternalDownloadToFile(url, file, 0, closeOutputStreamWhenFailed);
    }
    
    public void Download(string url, Stream stream, bool? closeOutputStreamWhenFailed = null)
    {
        InternalDownloadToStream(url, stream, 0, closeOutputStreamWhenFailed);
    }
}