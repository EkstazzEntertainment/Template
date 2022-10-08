namespace Game.Editor.AreaScenes
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Ekstazz.LevelBased.Logic;
    using UnityEditor;
    using UnityEditor.SceneManagement;
    using UnityEditor.SceneTemplate;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public static class AreaSceneAssetsController
    {
        private static readonly List<string> IgnoredAreas = new() { "Area_base" };

        public static List<string> FetchAreaNamesFromFolder()
        {
            return AssetDatabase.FindAssets("", new[] { AssetPaths.AreasFolderPath })
                .Select(AssetDatabase.GUIDToAssetPath)
                .Where(path => AssetDatabase.LoadAssetAtPath<Area>(path) != null)
                .Select(path => Tools.TrimExtension(Tools.SplitPath(path).assetName))
                .Where(name => !IgnoredAreas.Contains(name))
                .ToList();
        }

        public static string MakeSceneFromArea(string areaName)
        {
            var newScenePath = $"{AssetPaths.ScenesFolderPath}/{areaName}{AssetExtensions.Scene}";
            CreateSceneFromTemplate(areaName, newScenePath);
            InstantiatePrefabInScene(areaName, newScenePath);
            return newScenePath;
        }

        private static void CreateSceneFromTemplate(string sceneName, string scenePath)
        {
            var path = $"{AssetPaths.SceneTemplatesFolderPath}/Area/AreaTemplate{AssetExtensions.SceneTemplate}";
            var template = AssetDatabase.LoadAssetAtPath<SceneTemplateAsset>(path);
            SceneTemplateService.Instantiate(template, false, scenePath);
            DeleteLightningSettings(sceneName);
        }

        private static void DeleteLightningSettings(string sceneName)
        {
            var directoryMetaPath = $"{Application.dataPath}/_ROOT/Scenes/Areas/{sceneName}.meta";
            FileUtil.DeleteFileOrDirectory($"{AssetPaths.ScenesFolderPath}/{sceneName}");
            File.Delete(directoryMetaPath);
            AssetDatabase.Refresh();
        }

        private static void InstantiatePrefabInScene(string areaName, string scenePath)
        {
            var scene = EditorSceneManager.OpenScene(scenePath);
            var areaPrefabPath = $"{AssetPaths.AreasFolderPath}/{areaName}{AssetExtensions.Prefab}";
            var Area = AssetDatabase.LoadAssetAtPath<Area>(areaPrefabPath);
            Area.SetAreaId(areaName);
            var instance = PrefabUtility.InstantiatePrefab(Area.gameObject) as GameObject;
            SetAsWorldRootChild(instance, scene);
            SetLightningSettings();
            EditorSceneManager.SaveScene(scene);
        }

        private static void SetAsWorldRootChild(GameObject instance, Scene scene)
        {
            var worldRoot = GetWorldRoot(scene);
            instance.transform.parent = worldRoot.transform;
            instance.transform.SetSiblingIndex(0);
        }

        private static void SetLightningSettings()
        {
            var path = $"{AssetPaths.SceneTemplatesFolderPath}/Area/AreaTemplateLightningSettings.lighting";
            var lightningSettings = AssetDatabase.LoadAssetAtPath<LightingSettings>(path);
            Lightmapping.lightingSettings = lightningSettings;
        }

        private static GameObject GetWorldRoot(Scene scene)
        {
            return scene.GetRootGameObjects().First(go => go.name == "WorldRoot");
        }

        public static void ClearScenesFolder()
        {
            var paths = AssetDatabase.FindAssets("", new[] { AssetPaths.ScenesFolderPath })
                .Select(AssetDatabase.GUIDToAssetPath)
                .ToArray();

            AssetDatabase.DeleteAssets(paths, new List<string>());
        }

        public static string DeleteScene(string sceneName)
        {
            var scenePath = $"{AssetPaths.ScenesFolderPath}/{sceneName}{AssetExtensions.Scene}";
            AssetDatabase.DeleteAsset(scenePath);
            return scenePath;
        }
    }
}
