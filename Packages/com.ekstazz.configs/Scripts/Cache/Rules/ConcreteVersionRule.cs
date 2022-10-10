namespace Ekstazz.Configs.Cache
{
    internal class ConcreteVersionRule : ConfigRule
    {
        private readonly IVersionProvider versionProvider;
        
        
        public ConcreteVersionRule(IVersionProvider versionProvider, IConfigRule rule) : base(rule)
        {
            this.versionProvider = versionProvider;
        }
        
        public override bool ShouldReplace(ConfigMeta oldConfig, ConfigMeta newConfig)
        {
            if (IsConcreteVersion(newConfig))
            {
                return IsCurrentVersion(newConfig);
            }
            return base.ShouldReplace(oldConfig, newConfig);
        }
        
        private bool IsConcreteVersion(ConfigMeta config)
        {
            return config.type == ConfigMetaType.ConcreteVersion;
        }
        
        private bool IsCurrentVersion(ConfigMeta config)
        {
            var version = versionProvider.ParseVersionString(config.appVersion);
            return version == versionProvider.CurrentVersion;
        }
    }
}