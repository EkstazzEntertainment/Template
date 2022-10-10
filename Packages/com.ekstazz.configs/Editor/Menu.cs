namespace Ekstazz.Saves.Editor
{
    using System.IO;
    using Configs.Settings;
    using UnityEditor;
    using UnityEngine;

    
    public class Menu
    {
        [MenuItem("Ekstazz/Configs/Cache/Reset cache", false, 0)]
        private static void ResetCache()
        {
            File.Delete(Path.Combine(Application.persistentDataPath, "cache.dat"));
        }

        [MenuItem("Ekstazz/Configs/Cache/Settings")]
        private static void SelectSettings()
        {
            Selection.activeObject = CacheSettings.LoadOrCreate();
        }
    }
}