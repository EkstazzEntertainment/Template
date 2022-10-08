namespace Ekstazz.Configs
{
    using System;

    public class ConfigPropertyAttribute : Attribute
    {
        public ConfigPropertyAttribute()
        {
            Priority = DefaultPriority;
            PostProcessor = typeof(EmptyPostProcessor);
        }

        public const int DefaultPriority = 1;

        public int Priority { get; set; }

        public Type PostProcessor { get; set; }

        public string Key {get; set; }

        public bool IsMultiConfig { get; set; }
    }

    public class ConfigJsonPropertyAttribute : ConfigPropertyAttribute
    {
        
    }
}