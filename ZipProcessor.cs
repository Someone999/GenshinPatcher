﻿using System.IO.Compression;

namespace GenshinPatcher;

public static class ZipProcessor
{
    public static void Extract(ZipArchive archive, string directory)
    {
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        var archiveEntries = archive.Entries;
        foreach (var archiveEntry in archiveEntries)
        {
            string fullName = archiveEntry.FullName;
            if (fullName.StartsWith("\\") || fullName.StartsWith("/"))
            {
                fullName = fullName[1..];
            }
            string fullPath = Path.Combine(directory, fullName);
            
            if (string.IsNullOrEmpty(Path.GetFileName(fullPath)))
            {
                Directory.CreateDirectory(fullPath);
                continue;
            }
            
            var archiveFileStream = archiveEntry.Open();
            long bufferSize = 8192, total = archiveEntry.Length, readSize = 0;
            FileStream targetFile = File.Create(fullPath);
            byte[] buffer = new byte[bufferSize];
            Console.WriteLine($"正在解压文件: {fullName}");
            do
            {
                int realReadSize = archiveFileStream.Read(buffer);
                readSize += realReadSize;
                targetFile.Write(buffer);

            } while (readSize < total);
            targetFile.Close();
        }
    }
}