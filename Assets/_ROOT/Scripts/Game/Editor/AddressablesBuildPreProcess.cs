namespace Game.Editor
{
    using System;
    using UnityEditor;
    using UnityEditor.AddressableAssets.Settings;
    using UnityEngine;

    
    public class AddressablesBuildPreProcess
    {
        public void Execute()
        {
            Debug.Log("Building Addressables");
            try
            {
                Debug.Log("Started building");
                AddressableAssetSettings.BuildPlayerContent();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            Debug.Log("Finished building");
        }
    }

    public static class AddressableMenu
    {
        [MenuItem("Addressables/Build Addressables")]
        public static void Build()
        {
            var hook = new AddressablesBuildPreProcess();
            hook.Execute();
        }
    }
}