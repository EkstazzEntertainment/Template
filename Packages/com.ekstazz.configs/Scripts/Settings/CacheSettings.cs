namespace Ekstazz.Configs.Settings
{
    using UnityEditor;
    using UnityEngine;

    public class CacheSettings : ScriptableObject
    {
        [SerializeField]
        private bool isCacheEnabled;

        private const string Path = nameof(CacheSettings);

        public bool IsCacheEnabled => isCacheEnabled;

#if UNITY_EDITOR
        public void Save()
        {
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }

        public static CacheSettings LoadOrCreate()
        {
            var settings = Load();
            if (!settings)
            {
                settings = CreateInstance<CacheSettings>();
                AssetDatabase.CreateAsset(settings, $"Assets/Resources/{Path}.asset");
                AssetDatabase.SaveAssets();
            }

            return settings;
        }
#endif
        public static CacheSettings Load()
        {
            return Resources.Load<CacheSettings>(Path);
        }
    }
}