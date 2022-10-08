namespace Game.Editor.AreaScenes.Autoconfigs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Ekstazz.LevelBased.Logic;
    using UnityEditor;
    using UnityEngine;
    using Tools = Tools;

    
    public partial class LevelsConfigsEditorWindow : EditorWindow
    {
        private const string baseLevelConfigName = NameConstants.LevelConfigFileNameTemplate + LevelsConfigsController.BaseLevelConfigName;

        private const string levelIdPropertyName = "LevelId";
        private const string areaIdPropertyName = "AreaId";

        private const string areaPrefabName = "Area_";
        private const string areaPrefabsPath = "Assets/_ROOT/Prefabs/Areas";

        private const string pathToLevelConfigClassFile =
            "Assets/_ROOT/Scripts/Game/Proto/LevelBased/Logic/Levels/LevelConfig.cs";

        private const float standardButtonWidth = 80f;
        private const float doubledButtonWidth = standardButtonWidth * 2f;

        private GUIStyle levelConfigButtonSelectedStyle;

        private static (string levelConfigName, LevelConfig levelConfig) currentSelectedLevelConfig;
        private Dictionary<string, LevelConfig> loadedLevelConfigs = new();
        private PropertyInfo modifiedPropertyInfo;
        private int selectedLevelCopyFromIndex;
        private int copiesCount;

        private Dictionary<string, bool> selectedLevelConfigs = new();
        private bool isAllLevelConfigsSelected;

        private bool showLevelConfigContent;
        private Vector2 levelConfigContentScroll;

        private Vector2 levelConfigPropertiesScroll;
        private Vector2 levelConfigsNamesListScroll;

        private static bool isSelectedBaseLevelConfig =>
            currentSelectedLevelConfig.levelConfigName == baseLevelConfigName;

        [MenuItem("OSF/Configs/Levels")]
        public static void ShowWindow()
        {
            GetWindow(typeof(LevelsConfigsEditorWindow), false, NameConstants.LevelsConfigsEditorWindowName);
        }

        private void OnEnable()
        {
            minSize = new Vector2(600f, 400f);
            SetStyles();
        }

        private void SetStyles()
        {
            var selectedLevelConfigTexture = new Texture2D(1, 1);
            selectedLevelConfigTexture.SetPixel(1, 1,Color.black);
            selectedLevelConfigTexture.Apply();

            levelConfigButtonSelectedStyle = new GUIStyle("button")
            {
                normal =
                {
                    textColor = Color.white,
                    background = selectedLevelConfigTexture
                }
            };
        }

        private void LoadLevelConfigs()
        {
            loadedLevelConfigs.Clear();
            var levelsOrder = LevelsConfigsController.GetLevelsOrder();
            loadedLevelConfigs = Tools.GetLevelConfigsWithNames()
                .OrderBy(x => levelsOrder.MainLevelsOrder.IndexOf(x.Value.LevelId))
                .ToDictionary(x => x.Key, x => x.Value);
        }

        private void UpdateSelectedLevelConfigs()
        {
            foreach (KeyValuePair<string, LevelConfig> loadedLevelConfig in loadedLevelConfigs)
            {
                if (!selectedLevelConfigs.ContainsKey(loadedLevelConfig.Key))
                {
                    selectedLevelConfigs.Add(loadedLevelConfig.Key, false);
                }
            }

            TryRemoveDeletedLevelConfigs();

            void TryRemoveDeletedLevelConfigs()
            {
                try
                {
                    var removedLevelConfigs = selectedLevelConfigs.Where(x =>
                        !loadedLevelConfigs.ContainsKey(x.Key));
                    foreach (KeyValuePair<string, bool> selectedLevelConfigData in removedLevelConfigs)
                    {
                        selectedLevelConfigs.Remove(selectedLevelConfigData.Key);
                    }
                }
                catch (Exception _)
                {
                    // ignored
                }
            }
        }

        private void OnGUI()
        {
            InitializeSearchTypes();
            GUILayout.Space(5f);
            ShowConfigsPath();
            DrawSearch();
            if (!searchEnabled)
            {
                LoadLevelConfigs();
            }
            UpdateSelectedLevelConfigs();
            GUILayout.Space(5f);
            DrawWindowContent(loadedLevelConfigs);
        }

        private void ShowConfigsPath()
        {
            GUILayout.BeginHorizontal(EditorStyles.helpBox);
            GUILayout.Label("Path to level configs: ");
            GUILayout.Label(AssetPaths.LevelConfigsFolderPath);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        private void DrawWindowContent(Dictionary<string, LevelConfig> levelConfigs)
        {
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();

            DrawLevelConfigButton(baseLevelConfigName, LevelsConfigsController.GetBaseLevelConfig(), 200f);

            GUILayout.Space(5f);

            var isSelected = GUILayout.Toggle(isAllLevelConfigsSelected, "", GUILayout.Width(20f),
                GUILayout.Height(20f));
            if (isSelected != isAllLevelConfigsSelected)
            {
                SetAllLevelConfigsSelected(isSelected);
            }
            isAllLevelConfigsSelected = isSelected;
            DrawLevelConfigsList(levelConfigs);

            GUILayout.EndVertical();

            if (currentSelectedLevelConfig.levelConfigName != null &&
                !LevelsConfigsController.IsLevelConfigExists(currentSelectedLevelConfig.levelConfigName))
            {
                currentSelectedLevelConfig = (null, null);
            }
            TryDrawCurrentSelectedLevelConfigOptions();
            TryDrawSelectedLevelConfigsOptions();

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            void SetAllLevelConfigsSelected(bool isAllLevelConfigsSelected)
            {
                for (var i = 0; i < selectedLevelConfigs.Count; i++)
                {
                    var data = selectedLevelConfigs.ElementAt(i);
                    selectedLevelConfigs[data.Key] = isAllLevelConfigsSelected;
                }
            }

            void TryDrawSelectedLevelConfigsOptions()
            {
                if (CanDrawOptionsForCurrentSelectedLevelConfig())
                {
                    DrawDeleteSelectedLevelConfigsOption();
                }

                bool CanDrawOptionsForCurrentSelectedLevelConfig()
                {
                    var selectedLevelConfigsNumber = selectedLevelConfigs.Count(x => x.Value);
                    return selectedLevelConfigsNumber > 0f &&
                           (currentSelectedLevelConfig.levelConfig == null || selectedLevelConfigsNumber > 1);
                }

                void DrawDeleteSelectedLevelConfigsOption()
                {
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("Delete selected", GUILayout.Width(doubledButtonWidth)))
                    {
                        var selectedLevelConfigsCashed = selectedLevelConfigs.Where(x => x.Value)
                            .ToDictionary(x => x.Key, x => x.Value);

                        for (var i = 0; i < selectedLevelConfigsCashed.Count; i++)
                        {
                            var data = selectedLevelConfigsCashed.ElementAt(i);
                            LevelsConfigsController.DeleteLevelConfig(data.Key, loadedLevelConfigs[data.Key].LevelId);
                        }
                    }

                    GUILayout.EndHorizontal();
                }
            }
        }

        private void DrawLevelConfigsList(Dictionary<string, LevelConfig> levelConfigs)
        {
            levelConfigsNamesListScroll = GUILayout.BeginScrollView(levelConfigsNamesListScroll,
                EditorStyles.helpBox, GUILayout.Width(200f),
                GUILayout.MinHeight(200f), GUILayout.MaxHeight(600f));

            foreach (KeyValuePair<string, LevelConfig> levelConfigData in levelConfigs)
            {
                GUILayout.BeginHorizontal();

                selectedLevelConfigs[levelConfigData.Key] =
                    GUILayout.Toggle(selectedLevelConfigs[levelConfigData.Key], "", GUILayout.Width(20f),
                        GUILayout.Height(20f));

                if (!selectedLevelConfigs[levelConfigData.Key])
                {
                    isAllLevelConfigsSelected = false;
                }

                DrawLevelConfigButton(levelConfigData.Key, levelConfigData.Value, doubledButtonWidth);
                GUILayout.EndHorizontal();
            }

            GUILayout.EndScrollView();
        }

        private void DrawLevelConfigButton(string levelConfigName, LevelConfig levelConfig, float width = standardButtonWidth)
        {
            if (GUILayout.Button(levelConfigName, GetLevelConfigButtonStyle(levelConfigName), GUILayout.Width(width)))
            {
                GUI.FocusControl(null);
                currentSelectedLevelConfig = (levelConfigName, levelConfig);
            }
            
            GUIStyle GetLevelConfigButtonStyle(string levelConfigName)
            {
                return currentSelectedLevelConfig.levelConfig != null &&
                       currentSelectedLevelConfig.levelConfigName == levelConfigName
                    ? levelConfigButtonSelectedStyle
                    : new GUIStyle("button");
            }
        }

        // private void DrawSelectedConfigContent(string content)
        // {
        //     showLevelConfigContent =
        //         EditorGUILayout.BeginFoldoutHeaderGroup(showLevelConfigContent, "Show level config content");
        //     if (showLevelConfigContent)
        //     {
        //         levelConfigContentScroll =
        //             EditorGUILayout.BeginScrollView(levelConfigContentScroll, GUILayout.Height(125f));
        //         EditorGUILayout.LabelField(content, textFieldWrappedStyle, GUILayout.Height(120f));
        //         EditorGUILayout.EndScrollView();
        //     }
        //
        //     EditorGUILayout.EndFoldoutHeaderGroup();
        // }

        private void SaveChangesIn(string levelConfigName, LevelConfig levelConfig)
        {
            LevelsConfigsController.SaveLevelConfigFile(levelConfig, levelConfigName);
        }

        private void Revert(string levelConfigName, LevelConfig levelConfig, PropertyInfo propertyInfo)
        {
            var oldValue =
                propertyInfo.GetValue(
                    LevelsConfigsController.GetLevelConfig(levelConfigName));
            propertyInfo.SetValue(levelConfig, oldValue);
        }


    }
}