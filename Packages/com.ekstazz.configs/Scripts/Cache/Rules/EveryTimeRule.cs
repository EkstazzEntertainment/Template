namespace Ekstazz.Configs.Cache
{
    internal class EveryTimeRule : ConfigRule
    {
        public EveryTimeRule(IConfigRule rule) : base(rule)
        {
        }

        public override bool ShouldReplace(ConfigMeta oldConfig, ConfigMeta newConfig)
        {
            if (IsEveryType(newConfig))
            {
                return true;
            }
            return base.ShouldReplace(oldConfig, newConfig);
        }
        
        private bool IsEveryType(ConfigMeta config)
        {
            return config.type == ConfigMetaType.Every;
        }
    }
}