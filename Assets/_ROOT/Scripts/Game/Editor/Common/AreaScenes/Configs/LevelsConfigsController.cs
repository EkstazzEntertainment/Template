namespace Game.Editor.AreaScenes.Autoconfigs
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Ekstazz.LevelBased.Configs;
    using Ekstazz.LevelBased.Logic;
    using Newtonsoft.Json;
    using UnityEditor;
    using UnityEngine;
    using Tools = Tools;

    
    public static class LevelsConfigsController
    {
        public const string BaseLevelConfigName = "base";
        private const string LevelsOrderFileName = "levels_order";

        public static LevelsOrder GetLevelsOrder()
        {
            return Tools.GetJson<LevelsOrder>(LevelsOrderFileName,
                LevelsOrderFileName,
                AssetPaths.LevelsOrderFolderPath);
        }

        public static void TryCreateLevelConfigFile(string sceneName)
        {
            var baseLevelConfig = GetBaseLevelConfig();
            baseLevelConfig.LevelId = GetNewLevelConfigId();
            
            SaveLevelConfigFile(baseLevelConfig, baseLevelConfig.LevelId);
            AddToLevelsOrder(baseLevelConfig.LevelId);
            AssetDatabase.Refresh();
        }

        private static string GetNewLevelConfigId()
        {
            var levelNumText = "01";
            var levelConfigsNames = Tools.GetLevelConfigsNames();

            if (levelConfigsNames.Length > 0)
            {
                var levelNum = (levelConfigsNames.Select(levelConfigName =>
                {
                    var startIndex = levelConfigName.IndexOf('_');
                    var endIndex = levelConfigName.IndexOf('.');

                    var levelNum = levelConfigName.Substring(startIndex + 1, endIndex - startIndex - 1);

                    return Convert.ToInt32(levelNum, 10);
                }).OrderBy(levelNum => levelNum).Last() + 1);

                levelNumText = levelNum < 10 ? $"0{levelNum}" : levelNum.ToString();
            }

            return NameConstants.LevelConfigFileNameTemplate + levelNumText;
        }

        public static void CreateCopyOf(LevelConfig levelConfig)
        {
            var copy = new LevelConfig();
            var properties = GetLevelConfigsProperties();
            foreach (var propertyInfo in properties)
            {
                var value = propertyInfo.GetValue(levelConfig);
                propertyInfo.SetValue(copy, value);
            }

            copy.LevelId = GetNewLevelConfigId();
            SaveLevelConfigFile(copy, copy.LevelId);
            AddToLevelsOrder(copy.LevelId);
            AssetDatabase.Refresh();
        }

        public static LevelConfig GetBaseLevelConfig()
        {
            return GetLevelConfig(NameConstants.LevelConfigFileNameTemplate + BaseLevelConfigName);
        }

        public static LevelConfig GetLevelConfig(string levelConfigName)
        {
            var levelConfig = Tools.GetJson<List<LevelConfig>>(
                    levelConfigName,
                    NameConstants.LevelConfigFileNameTemplate,
                    AssetPaths.LevelConfigsFolderPath)
                .First();

            if (levelConfig != null)
            {
                return levelConfig;
            }

            Debug.LogError($"Failed to get level config file {levelConfigName}.");
            return null;
        }

        public static void SaveLevelConfigFile(LevelConfig levelConfig, string levelConfigFileName)
        {
            var fileName = AssetPaths.LevelConfigsFolderPath +
                           $"/{levelConfigFileName}{AssetExtensions.Json}";
            File.WriteAllText(fileName,
                JsonConvert.SerializeObject(new[] {levelConfig},
                    new JsonSerializerSettings
                    {
                        Formatting = Formatting.Indented,
                    }));
            AssetDatabase.Refresh();
            Debug.Log("Changes in level config file: <color=yellow>" + fileName + "</color>.");
        }

        private static void AddToLevelsOrder(string levelConfigName)
        {
            var levelsOrder = GetLevelsOrder();
            levelsOrder.MainLevelsOrder.Add(levelConfigName);

            File.WriteAllText(AssetPaths.LevelsOrderFolderPath + $"/{LevelsOrderFileName}{AssetExtensions.Json}",
                JsonConvert.SerializeObject(levelsOrder,
                    new JsonSerializerSettings
                    {
                        Formatting = Formatting.Indented,
                    }));
            AssetDatabase.Refresh();
            Debug.Log("Updated levels_order.json file.");
        }

        private static void RemoveFromLevelsOrder(string levelConfigName)
        {
            var levelsOrder = Tools.GetJson<LevelsOrder>(LevelsOrderFileName,
                LevelsOrderFileName, AssetPaths.LevelsOrderFolderPath);
            levelsOrder.MainLevelsOrder.Remove(levelConfigName);

            File.WriteAllText(AssetPaths.LevelsOrderFolderPath + $"/{LevelsOrderFileName}{AssetExtensions.Json}",
                JsonConvert.SerializeObject(levelsOrder,
                    new JsonSerializerSettings
                    {
                        Formatting = Formatting.Indented,
                    }));
            AssetDatabase.Refresh();
            Debug.Log("Updated levels_order.json file.");
        }

        // public static void RenameSceneInConfig(string oldSceneName, string newSceneName)
        // {
        //     var levelConfigs = Tools.GetLevelConfigsWithNames().Where(x =>
        //         x.Value.AreaId.Equals(oldSceneName)).ToList();
        //     foreach (var levelConfig in levelConfigs)
        //     {
        //         levelConfig.Value.AreaId = newSceneName;
        //         SaveLevelConfigFile(levelConfig.Value, levelConfig.Key);
        //     }
        //
        //     AssetDatabase.Refresh();
        //     EditorUtility.DisplayDialog("Level configs updated",
        //         $"Updated level configs that used the area name {oldSceneName}.", "Ok");
        // }

        // public static void DeleteLevelConfigsFor(string sceneName)
        // {
        //     var levelConfigsToChange = Tools.GetLevelConfigsWithNames()
        //         .Where(x => x.Value.AreaId.Equals(sceneName)).ToList();
        //
        //     if (ShowLevelConfigsDeletionDialog())
        //     {
        //         foreach (var levelConfig in levelConfigsToChange)
        //         {
        //             DeleteLevelConfig(levelConfig.Key, levelConfig.Value.LevelId);
        //         }
        //
        //         Debug.Log($"Level configs using the scene {sceneName} have been removed.");
        //     }
        //
        //     bool ShowLevelConfigsDeletionDialog()
        //     {
        //         return EditorUtility.DisplayDialog("Deleting level configs.",
        //             $"Are you sure you want to delete the {levelConfigsToChange.Count()} " +
        //             $"level configuration files that use the scene {sceneName}?",
        //             "Delete the level configs",
        //             "Do not delete any");
        //     }
        // }

        public static void DeleteLevelConfig(string levelConfigFileName, string levelId)
        {
            RemoveFromLevelsOrder(levelId);
            File.Delete(AssetPaths.LevelConfigsFolderPath + $"/{levelConfigFileName}{AssetExtensions.Json}");
            AssetDatabase.Refresh();
        }

        public static bool IsLevelConfigExists(string levelConfigFileName)
        {
            var fileName = AssetPaths.LevelConfigsFolderPath +
                           $"/{levelConfigFileName}{AssetExtensions.Json}";
            return File.Exists(fileName);
        }
        
        public static PropertyInfo[] GetLevelConfigsProperties()
        {
            return typeof(LevelConfig).GetProperties();
        }
        
        public static PropertyInfo GetProperty(string propertyName)
        {
            return typeof(LevelConfig).GetProperty(propertyName);
        }

        public static PropertyInfo[] GetLevelConfigsEditableProperties()
        {
            return LevelsConfigsController.GetLevelConfigsProperties()
                .Where(x => x.GetIndexParameters().Length == 0
                            && !Attribute.IsDefined(x, typeof(JsonIgnoreAttribute)))
                .ToArray();
        }
    }
}