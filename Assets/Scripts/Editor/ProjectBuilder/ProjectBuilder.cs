namespace Editor.ProjectBuilder
{
    using UnityEditor;
    using UnityEngine;

    public class ProjectBuilder : MonoBehaviour
    {
        [MenuItem("MyMenu/Do Something")]
        public static void Build()
        {
            string path = EditorUtility.SaveFolderPanel("Choose Location of Built Game", "", "");
            string[] levels = new string[] {"Assets/Scenes/AppScene.unity"};
            BuildPipeline.BuildPlayer(levels, path + "/BuiltGame.apk", BuildTarget.Android, BuildOptions.None);
        }
    }
    
}