namespace Ekstazz.Saves.DebugPanel
{
    using System.IO;
    using UnityEngine;

    
    public class ResetSavesDebugComponent : MonoBehaviour
    {
        public void ResetAllSaves(bool quitApplication)
        {
            ClearPlayerPrefs(false);
            ClearPersistentSaves(false);
            if (quitApplication)
            {
                QuitApplication();
            }
        }
        
        public void ClearPlayerPrefs(bool quitApplication)
        {
            PlayerPrefs.DeleteAll();
            if (quitApplication)
            {
                QuitApplication();
            }
        }
        
        public void ClearPersistentSaves(bool quitApplication)
        {
            File.Delete(Path.Combine(Application.persistentDataPath, "save.dat"));
            if (quitApplication)
            {
                QuitApplication();
            }
        }

        private void QuitApplication()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            UnityEngine.Application.Quit();
#endif
        }
    }
}