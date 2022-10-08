namespace Ekstazz.Saves.Editor
{
    using System.IO;
    using UnityEditor;
    using UnityEngine;

    public class Menu
    {
        [MenuItem("Ekstazz/Saves/Reset all", false, 0)]
        private static void ResetSaves()
        {
            ClearPlayerPrefs();
            ClearPersistentSaves();
        }
        
        [MenuItem("Ekstazz/Saves/Clear PlayerPrefs", false, 100)]
        private static void ClearPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
        }
        
        [MenuItem("Ekstazz/Saves/Clear Saves", false, 101)]
        private static void ClearPersistentSaves()
        {
            File.Delete(Path.Combine(Application.persistentDataPath, "save.dat"));
        }
    }
}