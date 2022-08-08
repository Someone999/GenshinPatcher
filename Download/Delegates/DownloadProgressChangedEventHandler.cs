namespace GenshinPatcher.Download.Delegates;

public delegate void DownloadProgressChangedEventHandler(double progress, long total, long downloaded);

public delegate void DownloadFailedEventHandler(string url, Exception? exception);

public delegate void DownloadCompletedEventHandler(string url, string targetFile);

public delegate void StreamDownloadCompletedEventHandler(string url, Stream stream);
