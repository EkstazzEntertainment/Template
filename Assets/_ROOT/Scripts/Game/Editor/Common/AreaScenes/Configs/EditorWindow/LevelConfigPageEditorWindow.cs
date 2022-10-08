namespace Game.Editor.AreaScenes.Autoconfigs
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Ekstazz.LevelBased.Logic;
    using UnityEditor;
    using UnityEngine;
    using Object = UnityEngine.Object;
    using Tools = Tools;

    public partial class LevelsConfigsEditorWindow
    {
        private void TryDrawCurrentSelectedLevelConfigOptions()
        {
            if (!CanDrawOptionsForCurrentSelectedLevelConfig())
            {
                return;
            }

            GUILayout.BeginVertical();
            // DrawSelectedConfigContent(JsonConvert.SerializeObject(new[] {currentSelectedLevelConfig.levelConfig}));
            GUILayout.BeginHorizontal();
            GUILayout.Label(currentSelectedLevelConfig.levelConfigName + AssetExtensions.Json,
                EditorStyles.boldLabel);

            if (GUILayout.Button("Highlight", GUILayout.Width(standardButtonWidth)))
            {
                EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath<Object>(
                    Tools.GetLevelConfigPath(currentSelectedLevelConfig.levelConfigName)));
            }

            if (GUILayout.Button("Save", GUILayout.Width(standardButtonWidth)))
            {
                SaveChangesIn(currentSelectedLevelConfig.levelConfigName, currentSelectedLevelConfig.levelConfig);
            }

            if (!isSelectedBaseLevelConfig)
            {
                if (GUILayout.Button("Delete", GUILayout.Width(standardButtonWidth)))
                {
                    LevelsConfigsController.DeleteLevelConfig(currentSelectedLevelConfig.levelConfigName,
                        currentSelectedLevelConfig.levelConfig.LevelId);
                    currentSelectedLevelConfig = (null, null);
                }
            }

            GUILayout.EndHorizontal();
            if (!isSelectedBaseLevelConfig)
            {
                DrawCopyingDataOption();
            }

            DrawCopiesCreationOption();
            levelConfigPropertiesScroll = GUILayout.BeginScrollView(levelConfigPropertiesScroll);
            DrawLevelConfigProperties();
            GUILayout.EndScrollView();
            GUILayout.EndVertical();

            #region Inner methods

            bool CanDrawOptionsForCurrentSelectedLevelConfig()
            {
                var selectedLevelConfigsNumber = selectedLevelConfigs.Count(x => x.Value);
                return currentSelectedLevelConfig.levelConfig != null
                       && (selectedLevelConfigsNumber <= 0f || (selectedLevelConfigsNumber == 1
                                                                && selectedLevelConfigs.First(x => x.Value).Key ==
                                                                currentSelectedLevelConfig.levelConfigName));
            }

            void DrawCopyingDataOption()
            {
                GUILayout.BeginHorizontal(EditorStyles.helpBox);
                GUILayout.Label("Copy data from");
                selectedLevelCopyFromIndex = EditorGUILayout.Popup(selectedLevelCopyFromIndex,
                    loadedLevelConfigs.Select(x => x.Key).ToArray());

                if (GUILayout.Button("Copy", GUILayout.Width(standardButtonWidth)))
                {
                    var selectedCopyFrom = loadedLevelConfigs.ElementAt(selectedLevelCopyFromIndex);
                    if (currentSelectedLevelConfig.levelConfigName != selectedCopyFrom.Key)
                    {
                        var properties = LevelsConfigsController.GetLevelConfigsEditableProperties();
                        for (var i = 0; i < properties.Length; i++)
                        {
                            var propertyInfo = properties[i];

                            if (propertyInfo.Name.Equals(levelIdPropertyName, StringComparison.OrdinalIgnoreCase))
                            {
                                continue;
                            }

                            var copyFromValue = propertyInfo.GetValue(selectedCopyFrom.Value);
                            propertyInfo.SetValue(currentSelectedLevelConfig.levelConfig, copyFromValue);
                            SaveChangesIn(currentSelectedLevelConfig.levelConfigName,
                                currentSelectedLevelConfig.levelConfig);
                        }
                    }
                }

                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }

            void DrawCopiesCreationOption()
            {
                GUILayout.BeginHorizontal(EditorStyles.helpBox);
                GUILayout.Label("Create");
                var newCopiesCountValue = GUILayout.TextField(copiesCount.ToString(), GUILayout.Width(80f));
                if (int.TryParse(newCopiesCountValue, out var copiesCountParsed))
                {
                    copiesCount = copiesCountParsed;
                }

                GUILayout.Label("copies");
                if (GUILayout.Button("Create", GUILayout.Width(standardButtonWidth)))
                {
                    for (var i = 0; i < copiesCount; i++)
                    {
                        LevelsConfigsController.CreateCopyOf(currentSelectedLevelConfig.levelConfig);
                    }
                }

                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }

            void DrawLevelConfigProperties()
            {
                var properties = LevelsConfigsController.GetLevelConfigsEditableProperties();
                for (var i = 0; i < properties.Length; i++)
                {
                    DrawLevelConfigProperty(properties[i]);
                }
            }

            void DrawLevelConfigProperty(PropertyInfo propertyInfo)
            {
                if (!LevelsConfigsController.IsLevelConfigExists(currentSelectedLevelConfig.levelConfigName))
                {
                    return;
                }

                var propertyType = propertyInfo.PropertyType;

                GUILayout.BeginHorizontal(EditorStyles.helpBox);
                GUILayout.Label($"{propertyInfo.Name}:", EditorStyles.boldLabel, GUILayout.Width(120f));

                if (propertyInfo.Name.Equals(levelIdPropertyName, StringComparison.OrdinalIgnoreCase))
                {
                    GUILayout.Label($"{propertyInfo.GetValue(currentSelectedLevelConfig.levelConfig)}",
                        EditorStyles.boldLabel,
                        GUILayout.MinWidth(120f));

                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    return;
                }

                if (propertyInfo.Name.Equals(areaIdPropertyName, StringComparison.OrdinalIgnoreCase))
                {
                    var areasIds = GetAreasIds();
                    areasIds.Reverse();
                    var areaIdIndex =
                        areasIds.IndexOf((string) propertyInfo.GetValue(currentSelectedLevelConfig.levelConfig));
                    areaIdIndex = EditorGUILayout.Popup(areaIdIndex, areasIds.ToArray());
                    propertyInfo.SetValue(currentSelectedLevelConfig.levelConfig, areasIds[areaIdIndex]);

                    List<string> GetAreasIds()
                    {
                        return AssetDatabase.FindAssets(areaPrefabName, new string[] {areaPrefabsPath})
                            .Select(AssetDatabase.GUIDToAssetPath)
                            .Select(x =>
                            {
                                var area = (Area) AssetDatabase.LoadAssetAtPath(x, typeof(Area));
                                return area.AreaId;
                            })
                            .ToList();
                    }
                }
                else
                {
                    TryDrawStringProperty(propertyInfo, propertyType, currentSelectedLevelConfig.levelConfig);
                    TryDrawBoolProperty(propertyInfo, propertyType, currentSelectedLevelConfig.levelConfig);
                    TryDrawEnumProperty(propertyInfo, propertyType, currentSelectedLevelConfig.levelConfig);

                    try
                    {
                        TryDrawNumericProperty(propertyInfo, propertyType, currentSelectedLevelConfig.levelConfig);
                    }
                    catch (Exception e)
                    {
                        Revert(currentSelectedLevelConfig.levelConfigName, currentSelectedLevelConfig.levelConfig,
                            propertyInfo);
                        Debug.LogError(e);
                    }

                    if (isSelectedBaseLevelConfig)
                    {
                        if (GUILayout.Button("Delete", GUILayout.Width(standardButtonWidth)))
                        {
                            var classText = ReadLevelConfigClassFile();
                            var indexOfPropertyName = classText.IndexOf(propertyInfo.Name, StringComparison.Ordinal);
                            var indexOfPropertyDeclarationStart =
                                classText.LastIndexOf("}", indexOfPropertyName, StringComparison.Ordinal);
                            var indexOfPropertyDeclarationEnd =
                                classText.IndexOf("}", indexOfPropertyName, StringComparison.Ordinal);

                            var modifiedLevelConfigClass = classText.Remove(indexOfPropertyDeclarationStart,
                                indexOfPropertyDeclarationEnd - indexOfPropertyDeclarationStart);
                            RewriteLevelConfigClassFile(pathToLevelConfigClassFile, modifiedLevelConfigClass);
                            RemovePropertyFromLevelConfigs(propertyInfo.Name);
                            AssetDatabase.Refresh();

                            string ReadLevelConfigClassFile()
                            {
                                return File.ReadAllText(pathToLevelConfigClassFile);
                            }

                            void RewriteLevelConfigClassFile(string path, string content)
                            {
                                File.WriteAllText(path, content);
                            }

                            void RemovePropertyFromLevelConfigs(string propertyName)
                            {
                                var levelConfigsNames = Tools.GetLevelConfigsNames();
                                for (var i = 0; i < levelConfigsNames.Length; i++)
                                {
                                    Debug.Log($"{AssetPaths.LevelConfigsFolderPath}/{levelConfigsNames[i]}");
                                    ChangeLevelConfig($"{AssetPaths.LevelConfigsFolderPath}/{levelConfigsNames[i]}");
                                }
                                Debug.Log($"{AssetPaths.LevelConfigsFolderPath}/{baseLevelConfigName}{AssetExtensions.Json}");
                                ChangeLevelConfig(
                                    $"{AssetPaths.LevelConfigsFolderPath}/{baseLevelConfigName}{AssetExtensions.Json}");

                                void ChangeLevelConfig(string path)
                                {
                                    var content = File.ReadAllText(path);

                                    var index = content.IndexOf(propertyName, StringComparison.OrdinalIgnoreCase);
                                    Debug.Log(index);

                                    if (index < 0)
                                    {
                                        return;
                                    }

                                    var start = content.LastIndexOf(",", index, StringComparison.Ordinal);
                                    Debug.Log(start);
                                    var end = content.IndexOf(",", index, StringComparison.Ordinal);

                                    if (end < 0)
                                    {
                                        end = content.IndexOf("}", index, StringComparison.Ordinal);
                                    }

                                    Debug.Log(end);

                                    var newContent = content.Remove(start, end - start);

                                    File.WriteAllText(path, newContent);
                                }
                            }
                        }
                    }
                }

                if (GUILayout.Button("Revert", GUILayout.Width(standardButtonWidth)))
                {
                    Revert(currentSelectedLevelConfig.levelConfigName, currentSelectedLevelConfig.levelConfig,
                        propertyInfo);
                }

                if (GUILayout.Button("Default", GUILayout.Width(standardButtonWidth)))
                {
                    var oldValue = propertyInfo.GetValue(LevelsConfigsController.GetBaseLevelConfig());
                    propertyInfo.SetValue(currentSelectedLevelConfig.levelConfig, oldValue);
                    SaveChangesIn(currentSelectedLevelConfig.levelConfigName, currentSelectedLevelConfig.levelConfig);
                }

                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                #region Drawing properties methods

                void TryDrawStringProperty(PropertyInfo propertyInfo, Type propertyType, object modifiedObject)
                {
                    if (propertyType != typeof(string))
                    {
                        return;
                    }

                    var newValue = GUILayout.TextField($"{propertyInfo.GetValue(modifiedObject)}",
                        EditorStyles.textField,
                        GUILayout.MinWidth(120f),
                        GUILayout.MaxWidth(450f));
                    propertyInfo.SetValue(modifiedObject, newValue);
                }

                void TryDrawBoolProperty(PropertyInfo propertyInfo, Type propertyType, object modifiedObject)
                {
                    if (propertyType != typeof(bool))
                    {
                        return;
                    }

                    var newValue = GUILayout.Toggle((bool) propertyInfo.GetValue(modifiedObject),
                        "",
                        GUILayout.Width(20f),
                        GUILayout.Height(20f));
                    propertyInfo.SetValue(modifiedObject, newValue);
                }

                void TryDrawEnumProperty(PropertyInfo propertyInfo, Type propertyType, object modifiedObject)
                {
                    if (!propertyType.IsEnum)
                    {
                        return;
                    }

                    var newValue =
                        EditorGUILayout.EnumPopup((Enum) propertyInfo.GetValue(modifiedObject));
                    propertyInfo.SetValue(modifiedObject, newValue);
                }

                void TryDrawNumericProperty(PropertyInfo propertyInfo, Type propertyType, object modifiedObject)
                {
                    if (propertyType.IsEnum || !propertyType.IsNumeric())
                    {
                        return;
                    }

                    var inputValue = EditorGUILayout.TextField(
                        propertyInfo.GetValue(modifiedObject).ToString(),
                        GUILayout.MinWidth(80f),
                        GUILayout.MaxWidth(450f));
                    var newValue = Convert.ChangeType(inputValue, propertyType);
                    propertyInfo.SetValue(modifiedObject, newValue);
                }

                #endregion
            }

            #endregion
        }
    }
}