namespace Game.Editor.AreaScenes.Autoconfigs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEditor;
    using UnityEditor.IMGUI.Controls;
    using UnityEngine;
    using Tools = Tools;

    public partial class LevelsConfigsEditorWindow
    {
        private string searchString = "";
        private SearchField searchField;
        private int selectedLevelConfigSearchType;
        private List<string> searchTypes = new();
        private const string basicSearchType = "File name";
        private bool searchEnabled;

        private void InitializeSearchTypes()
        {
            searchTypes.Clear();
            searchTypes.Add(basicSearchType);
            searchTypes.AddRange(LevelsConfigsController.GetLevelConfigsEditableProperties()
                .Select(x => x.Name));
        }

        private void DrawSearch()
        {
            GUILayout.BeginVertical();
            selectedLevelConfigSearchType =
                EditorGUILayout.Popup("Search by", selectedLevelConfigSearchType, searchTypes.ToArray());

            searchField ??= new SearchField();
            searchString = searchField.OnGUI(searchString);
            searchEnabled = searchString is {Length: > 0};
            if (searchEnabled)
            {
                if (selectedLevelConfigSearchType == 0)
                {
                    SearchByName(searchString);
                }
                else
                {
                    SearchByParameter(searchTypes[selectedLevelConfigSearchType], searchString);
                }
            }

            GUILayout.EndVertical();
        }

        private void SearchByName(string desiredString)
        {
            loadedLevelConfigs.Clear();
            loadedLevelConfigs = Tools.GetLevelConfigsWithNames()
                .Where(x => x.Key.Contains(desiredString, StringComparison.OrdinalIgnoreCase))
                .ToDictionary(x => x.Key, x => x.Value);
        }

        private void SearchByParameter(string propertyName, string desiredString)
        {
            loadedLevelConfigs.Clear();
            loadedLevelConfigs = Tools.GetLevelConfigsWithNames()
                .Where(x => LevelsConfigsController.GetProperty(propertyName)
                    .GetValue(x.Value)
                    .ToString()
                    .Contains(desiredString, StringComparison.OrdinalIgnoreCase))
                .ToDictionary(x => x.Key, x => x.Value);
        }
    }
}