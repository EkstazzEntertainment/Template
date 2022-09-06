namespace Editor.ProjectBuilder
{
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;


    public class ProjectBuilder : MonoBehaviour
    {
        [MenuItem("Build/Android Build")]
        public static void BuildAndroid()
        {
            string path = EditorUtility.SaveFolderPanel("Choose Location of Built Game", "", "");
            string[] levels = GetAllScenes();
            EditorUserBuildSettings.development = false;
            EditorUserBuildSettings.buildAppBundle = true;
            BuildPipeline.BuildPlayer(levels, path + "/BuiltGame.aab", BuildTarget.Android, BuildOptions.None);
        }
        
        [MenuItem("Build/iOS Build")]
        public static void BuildIOS()
        {
            
        }

        private static string[] GetAllScenes()
        {
            List<string> scenes = new List<string>();
            foreach(EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
            {
                if (scene.enabled)
                {
                    scenes.Add(scene.path);
                }
            }

            return scenes.ToArray();
        }

        private void GetParamsFile()
        {
            
        }
    }
    
}