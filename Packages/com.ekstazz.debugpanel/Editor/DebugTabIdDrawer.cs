namespace Packages.Debug.Editor
{
    using System;
    using Ekstazz.DebugPanel;
    using UnityEditor;
    using UnityEngine;
    [CustomPropertyDrawer(typeof(DebugTabId))]
    public class DebugTabIdDrawer : PropertyDrawer
    {
        private string[] ids;
        private int index;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            LoadTabIds();
            CalculateIndex(property.stringValue);
            CreatePopup(position, property);
        }
        private void LoadTabIds()
        {
            var path = $"Settings/{nameof(DebugTabSettings)}";
            ids =  Resources.Load<DebugTabSettings>(path).ids.ToArray();
        }
        private void CalculateIndex(string currentValue)
        {
            index = Mathf.Max(0, Array.IndexOf(ids, currentValue));
        }
        private void CreatePopup(Rect position, SerializedProperty property)
        {
            index = EditorGUI.Popup(position, property.displayName, index, ids);
            property.stringValue = ids[index];
        }
    }
}