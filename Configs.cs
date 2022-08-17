using System.ComponentModel;
using GenshinPatchTools.Config;

namespace GenshinPatcher;

public static class Configs
{
    public static void Init()
    {
        string configFile = "config.json";
        if (!File.Exists(configFile))
        {
            File.CreateText(configFile).Close();
        }

        Config = new JsonConfig(configFile);
    }
    
    public static IConfigElement? Config { get; private set; }
}