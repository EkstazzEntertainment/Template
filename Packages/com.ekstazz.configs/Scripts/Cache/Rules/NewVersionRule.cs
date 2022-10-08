namespace Ekstazz.Configs.Cache
{
    internal class NewVersionRule : ConfigRule
    {
        private readonly IVersionProvider versionProvider;
        
        public NewVersionRule(IVersionProvider versionProvider, IConfigRule rule) : base(rule)
        {
            this.versionProvider = versionProvider;
        }
        
        public override bool ShouldReplace(ConfigMeta oldConfig, ConfigMeta newConfig)
        {
            if (IsSingleVersion(newConfig) && versionProvider.IsNewVersion)
            {
                return true;
            }
            return base.ShouldReplace(oldConfig, newConfig);
        }

        private bool IsSingleVersion(ConfigMeta config)
        {
            return config.type == ConfigMetaType.Version;
        }
    }
}