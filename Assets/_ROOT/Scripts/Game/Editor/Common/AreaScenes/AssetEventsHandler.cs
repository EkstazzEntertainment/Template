namespace Game.Editor.AreaScenes
{
    using System.Linq;
    using System.Threading.Tasks;
    using Ekstazz.LevelBased.Logic;
    using UnityEditor;
    using UnityEditor.SceneManagement;
    using UnityEngine;

    public class AssetEventsHandler : AssetModificationProcessor
    {
        private static async void OnWillCreateAsset(string assetPath)
        {
            await Task.Yield();
            if (ValidatePrefabPath(assetPath, out var sceneName) && !Tools.IsSceneExist(sceneName))
            {
                var (currentSceneName, currentScenePath) = Tools.GetCurrentSceneInfo();
                var newScenePath = AreaSceneAssetsController.MakeSceneFromArea(sceneName);
                BuildSettingsController.AddScene(newScenePath);
                BuildSettingsController.SaveOnDisk();
                if (Tools.IsSceneExist(currentSceneName))
                {
                    EditorSceneManager.OpenScene(currentScenePath);
                }
            }
        }

        private static bool ValidatePrefabPath(string path, out string sceneName)
        {
            sceneName = "";
            var (folderName, assetName) = Tools.SplitPath(path);
            if (folderName == AssetPaths.AreasFolderPath && assetName.EndsWith(AssetExtensions.Prefab))
            {
                sceneName = Tools.TrimExtension(assetName);
                return true;
            }

            return false;
        }

        private static AssetDeleteResult OnWillDeleteAsset(string assetPath, RemoveAssetOptions options)
        {
            if (ValidatePrefabPath(assetPath, out var sceneName))
            {
                var deletedScenePath = AreaSceneAssetsController.DeleteScene(sceneName);
                BuildSettingsController.DeleteScene(deletedScenePath);
                BuildSettingsController.SaveOnDisk();
            }

            return AssetDeleteResult.DidNotDelete;
        }

        private static AssetMoveResult OnWillMoveAsset(string sourcePath, string destinationPath)
        {
            var (srcFolder, srcAssetName) = Tools.SplitPath(sourcePath);
            var (dstFolder, dstAssetName) = Tools.SplitPath(destinationPath);
            if (CheckIfRenaming(srcFolder, dstFolder) &&
                ValidatePrefabPath(sourcePath, out var oldSceneName))
            {
                var newSceneName = Tools.TrimExtension(dstAssetName);
                RenameScene(oldSceneName, newSceneName);
            }

            return AssetMoveResult.DidNotMove;
        }

        private static bool CheckIfRenaming(string sourceFolder, string destinationFolder)
        {
            return sourceFolder == destinationFolder;
        }

        private static void RenameScene(string oldSceneName, string newSceneName)
        {
            var oldScenePath = $"{AssetPaths.ScenesFolderPath}/{oldSceneName}{AssetExtensions.Scene}";
            var sceneNameWithExtension = $"{newSceneName}{AssetExtensions.Scene}";
            AssetDatabase.RenameAsset(oldScenePath, sceneNameWithExtension);
        }

        private static string[] OnWillSaveAssets(string[] paths)
        {
            if (paths.All(p => !ValidateScenePath(p)))
            {
                return paths;
            }
            var areas = Object.FindObjectsOfType<Area>();
            foreach (var area in areas)
            {
                PrefabUtility.ApplyPrefabInstance(area.gameObject, InteractionMode.AutomatedAction);
            }
            return paths;
        }

        private static bool ValidateScenePath(string path)
        {
            var (folderName, assetName) = Tools.SplitPath(path);
            return folderName == AssetPaths.ScenesFolderPath && assetName.EndsWith(AssetExtensions.Scene);
        }
    }
}
