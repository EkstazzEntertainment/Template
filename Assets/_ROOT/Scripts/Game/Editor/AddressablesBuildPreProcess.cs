namespace Game.Editor
{
    using System;
    using TatemGames.Editor.Build;
    using UnityEditor.AddressableAssets.Settings;
    using UnityEngine;

    
    public class AddressablesBuildPreProcess : IPreProjectBuilderAction
    {
        public void Execute()
        {
            Debug.Log("Addressables preprocessor running...");
            try
            {
                Debug.Log("Building Addressables content");
                AddressableAssetSettings.BuildPlayerContent();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            Debug.Log("Addressables preprocessor finished");
        }
    }
}