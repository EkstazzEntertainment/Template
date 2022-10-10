namespace Ekstazz.Configs.Cache
{
    using System;

    
    [Serializable]
    public class Config
    {
        public ConfigMeta meta;
        
        public string value;
        
        public Config(string value, ConfigMeta meta)
        {
            this.value = value;
            this.meta = meta;
        }
    }
    
    [Serializable]
    public class ConfigMeta
    {
        public string type = ConfigMetaType.Forever;
        public int priority;
        public string appVersion;
        public string abTestName;
        public string abVariant;
    }

    internal static class ConfigMetaType
    {
        public const string Forever = "forever";
        public const string Version = "single_version";
        public const string ConcreteVersion = "concrete_version";
        public const string Every = "every_time";
    }
}