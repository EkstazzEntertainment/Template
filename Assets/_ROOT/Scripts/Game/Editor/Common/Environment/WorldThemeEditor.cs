namespace Game.Editor.Environment
{
    using Ekstazz.ProtoGames.Level.World;
    using UnityEditor;
    using UnityEngine;
    using Editor = UnityEditor.Editor;

    
    [CustomEditor(typeof(WorldTheme))]
    public class WorldThemeEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var targetType = (WorldTheme) target;
 
            if(GUILayout.Button("APPLY ENV SETTINGS", GUILayout.Height(40)))
            {
                targetType.ApplyEnvironmentSettings();
            }
        }
    }
}