namespace Game.Editor.AreaScenes
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Ekstazz.LevelBased.Logic;
    using Newtonsoft.Json;
    using UnityEditor;
    using UnityEditor.SceneManagement;
    using UnityEngine;

    
    public static class Tools
    {
        private const string ScenesFolderPath = "Assets/_ROOT/Scenes";

        private const string LevelConfigNameRegex = @"\bLevel_\d+\" + AssetExtensions.Json + "$";

        public static (string folderName, string assetName) SplitPath(string fullPath)
        {
            var pathElementsList = fullPath.Split("/");
            var assetName = pathElementsList[^1];
            var folderName = string.Join("/", pathElementsList[..^1]);
            return (folderName, assetName);
        }

        public static string TrimExtension(string nameWithExtension)
        {
            return nameWithExtension.Split('.')[0];
        }

        public static bool IsSceneExist(string sceneName)
        {
            var foundScenePaths = GetGuidsToAllAssetsAtPath(sceneName, ScenesFolderPath)
                .Where(path => SplitPath(path).assetName == $"{sceneName}{AssetExtensions.Scene}")
                .ToArray();
            return foundScenePaths.Length != 0;
        }

        public static (string sceneName, string scenePath) GetCurrentSceneInfo()
        {
            var currentScene = EditorSceneManager.GetActiveScene();
            var currentSceneName = currentScene.name;
            var currentScenePath = currentScene.path;
            return (currentSceneName, currentScenePath);
        }

        public static string GetLevelConfigPath(string levelConfigName)
        {
            return GetGuidsToAllAssetsAtPath(NameConstants.LevelConfigFileNameTemplate,
                    AssetPaths.LevelConfigsFolderPath)
                .First(path =>
                    Regex.Match(SplitPath(path).assetName, levelConfigName, RegexOptions.IgnoreCase).Success);
        }
        public static Dictionary<string, LevelConfig> GetLevelConfigsWithNames()
        {
            return GetLevelConfigsNames().Select(foundedLevelConfig =>
            {
                var levelConfigFileName = foundedLevelConfig.Substring(0, foundedLevelConfig.IndexOf('.'));
                var levelConfigData = GetJson<List<LevelConfig>>(levelConfigFileName,
                    NameConstants.LevelConfigFileNameTemplate, AssetPaths.LevelConfigsFolderPath)?.First();
                
                return (levelConfigFileName, levelConfigData);
            })
                .Where(x => x.Item2 != null)
                .ToDictionary(x => x.levelConfigFileName, x => x.Item2);
        }

        public static string[] GetLevelConfigsNames()
        {
            var foundLevelConfigs =
                GetGuidsToAllAssetsAtPath(NameConstants.LevelConfigFileNameTemplate, AssetPaths.LevelConfigsFolderPath)
                    .Where(path =>
                        Regex.Match(SplitPath(path).assetName, LevelConfigNameRegex, RegexOptions.IgnoreCase).Success)
                    .Select(path => SplitPath(path).assetName)
                    .ToArray();

            return foundLevelConfigs;
        }

        public static T GetJson<T>(string jsonFileName, string filter, string pathToFile) where T : class
        {
            var jsonFileNameRegex = $@"{jsonFileName}{AssetExtensions.Json}";
            Debug.Log(jsonFileNameRegex + "            " + filter + "            " + pathToFile);
            var foundedPath =
                GetGuidsToAllAssetsAtPath(filter, pathToFile).First(
                    path => Regex.Match(SplitPath(path).assetName, jsonFileNameRegex, RegexOptions.IgnoreCase)
                        .Success);
            try
            {
                using var streamReader = new StreamReader(foundedPath);
                return JsonConvert.DeserializeObject<T>(streamReader.ReadToEnd()) ?? throw new
                    InvalidOperationException();
            }
            catch (Exception exception)
            {
                Debug.LogError(
                    $"Caught: Error getting json file: {jsonFileName}{AssetExtensions.Json}. Info: {exception}");
            }

            return null;
        }

        public static IEnumerable<string> GetGuidsToAllAssetsAtPath(string filter, string path)
        {
            return AssetDatabase
                .FindAssets(filter, new[] {path})
                .Select(AssetDatabase.GUIDToAssetPath);
        }
    }
}