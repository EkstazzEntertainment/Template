namespace Ekstazz.Configs.Cache
{
    internal class TypeRule : ConfigRule
    {
        public TypeRule(IConfigRule rule) : base(rule)
        {
        }
        
        public override bool ShouldReplace(ConfigMeta oldConfig, ConfigMeta newConfig)
        {
            var oldTypePriority = GetTypePriority(oldConfig.type);
            var newTypePriority = GetTypePriority(newConfig.type);
            if (newTypePriority > oldTypePriority)
            {
                return true;
            }
            if (newTypePriority < oldTypePriority)
            {
                return false;
            }
            return base.ShouldReplace(oldConfig, newConfig);
        }
        
        private int GetTypePriority(string type)
        {
            switch (type)
            {
                case ConfigMetaType.Forever: return 1;
                case ConfigMetaType.Version: return 2;
                case ConfigMetaType.ConcreteVersion: return 3;
                case ConfigMetaType.Every: return 4;
            }
            return 0;
        }
    }
}