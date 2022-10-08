namespace Game.Editor.AreaScenes
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEditor;

    public class BuildSettingsController : EditorWindow
    {
        private static readonly List<string> DefaultScenes = new() { 
            "Assets/_ROOT/Scenes/Splash.unity", 
            "Assets/_ROOT/Scenes/Loader.unity",
            "Assets/_ROOT/Scenes/Game.unity",
            "Assets/_ROOT/Scenes/UiEdit.unity"
        };
        
        public static void AddScene(string scenePath)
        {
            var editorBuildSettingsScenes = EditorBuildSettings.scenes.ToList();
            editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(scenePath, true));
            EditorBuildSettings.scenes = editorBuildSettingsScenes.ToArray();
        }

        public static void DeleteScene(string scenePath)
        {
            var editorBuildSettingsScenes = EditorBuildSettings.scenes.ToList();
            editorBuildSettingsScenes.RemoveAll(scene => scene.path == scenePath);
            EditorBuildSettings.scenes = editorBuildSettingsScenes.ToArray();
        }

        public static void ResetBuildSettings()
        {
            var scenes = DefaultScenes
                .Select(path => new EditorBuildSettingsScene(path, true))
                .ToArray();
            
            EditorBuildSettings.scenes = scenes;
        }
        
        public static void SaveOnDisk()
        {
            AssetDatabase.SaveAssets();
        }
    }
}