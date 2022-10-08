namespace Ekstazz.Configs.Cache
{
    internal class PriorityRule : ConfigRule
    {
        public PriorityRule(IConfigRule rule) : base(rule)
        {
        }
        
        public override bool ShouldReplace(ConfigMeta oldConfig, ConfigMeta newConfig)
        {
            if (newConfig.priority > oldConfig.priority)
            {
                return true;
            }
            if (newConfig.priority < oldConfig.priority)
            {
                return false;
            }
            return base.ShouldReplace(oldConfig, newConfig);
        }
    }
}