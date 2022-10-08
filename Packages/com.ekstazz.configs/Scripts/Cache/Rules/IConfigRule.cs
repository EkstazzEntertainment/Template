namespace Ekstazz.Configs.Cache
{
    internal interface IConfigRule
    {
        IConfigRule NextRule { get; }

        bool ShouldReplace(ConfigMeta oldConfig, ConfigMeta newConfig);
    }

    internal abstract class ConfigRule : IConfigRule
    {
        public IConfigRule NextRule { get; }
        
        public ConfigRule(IConfigRule rule)
        {
            NextRule = rule;
        }

        public virtual bool ShouldReplace(ConfigMeta oldConfig, ConfigMeta newConfig)
        {
            return NextRule?.ShouldReplace(oldConfig, newConfig) ?? false;
        }
    }
}