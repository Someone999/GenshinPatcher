namespace GenshinPatcher.Tools;

public static class FileTools
{
    public static void RecurseDelete(string path)
    {
        string[] files = Directory.GetFiles(path);
        if (Directory.Exists(path))
        {
            string[] directories = Directory.GetDirectories(path);
            foreach (var directory in directories)
            {
                RecurseDelete(directory);
                Directory.Delete(directory);
            }
        }

        if (files.Length == 0)
        {
            return;
        }
        
        foreach (var file in files)
        {
            File.Delete(file);
        }
    }
}