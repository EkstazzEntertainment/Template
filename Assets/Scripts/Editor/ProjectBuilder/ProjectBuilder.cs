namespace Editor.ProjectBuilder
{
    using System.Collections.Generic;
    using App.DataBase;
    using App.DataBase.Structures;
    using UnityEditor;


    public class ProjectBuilder
    {
        [MenuItem("Build/Android Build")]
        public static void BuildAndroid()
        {
            GetParamsFile<BuildParams>(out var buildParams);
            string[] levels = GetAllScenes();
            EditorUserBuildSettings.development = false;
            EditorUserBuildSettings.buildAppBundle = true;
            BuildPipeline.BuildPlayer(levels, "/Users/yuriialeksanian/Desktop" + "/BuiltGame.aab", BuildTarget.Android, BuildOptions.None);
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

        private static void GetParamsFile<T>(out T buildParams)
        {
            var dataBaseHelper = new DataBaseHelper();
            dataBaseHelper.TextIntoType("buildparams.txt", out T parsedResult);
            buildParams = parsedResult;
        }
    }
    
}