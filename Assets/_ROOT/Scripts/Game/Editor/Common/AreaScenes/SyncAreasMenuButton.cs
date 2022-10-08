namespace Game.Editor.AreaScenes
{
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEditor.SceneManagement;

    public class SyncAreasMenuButton : Editor
    {
        [MenuItem("OSF/Scenes/Synchronize")]
        private static void SynchronizeScenes()
        {
            BuildSettingsController.ResetBuildSettings();
            AreaSceneAssetsController.ClearScenesFolder();
            var areaNames = AreaSceneAssetsController.FetchAreaNamesFromFolder();
            var (currentSceneName, currentScenePath) = Tools.GetCurrentSceneInfo();
            CreateScenesForAreas(areaNames);
            if (Tools.IsSceneExist(currentSceneName))
            {
                EditorSceneManager.OpenScene(currentScenePath);
            }
            BuildSettingsController.SaveOnDisk();
        }

        private static void CreateScenesForAreas(List<string> areaNames)
        {
            var (currentSceneName, currentScenePath) = Tools.GetCurrentSceneInfo();
            foreach (var areaName in areaNames)
            {
                var newScenePath = AreaSceneAssetsController.MakeSceneFromArea(areaName);
                BuildSettingsController.AddScene(newScenePath);
            }
            if (Tools.IsSceneExist(currentSceneName))
            {
                EditorSceneManager.OpenScene(currentScenePath);
            }
        }
    }
}
