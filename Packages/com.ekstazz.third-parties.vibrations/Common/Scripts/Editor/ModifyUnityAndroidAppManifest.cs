namespace MoreMountains.NiceVibrations
{
    using System.IO;
    using System.Text;
    using UnityEditor.Android;

    public class ModifyAndroidManifest : IPostGenerateGradleAndroidProject
    {
        public int callbackOrder => 1;

        public void OnPostGenerateGradleAndroidProject(string basePath)
        {
            // If needed, add condition checks on whether you need to run the modification routine.
            // For example, specific configuration/app options enabled
            var androidManifest = new AndroidManifest(GetManifestPath(basePath));
            androidManifest.SetVibratePermission();
            androidManifest.Save();
        }

        private string GetManifestPath(string basePath)
        {
            return new StringBuilder(basePath)
                .Append(Path.DirectorySeparatorChar).Append("src")
                .Append(Path.DirectorySeparatorChar).Append("main")
                .Append(Path.DirectorySeparatorChar).Append("AndroidManifest.xml")
                .ToString();
        }
    }
}
