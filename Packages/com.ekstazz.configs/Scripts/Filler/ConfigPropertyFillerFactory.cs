namespace Ekstazz.Configs
{
    using System;
    using System.Reflection;
    using Zenject;

    internal class ConfigPropertyFillerFactory
    {
        [Inject]
        public BackedUpConfigApplier ConfigApplier { get; set; }

        [Inject]
        public IConfigParser Parser { get; set; }

        public virtual ConfigPropertyFiller CreateFillerFor(PropertyInfo propertyInfo)
        {
            if (propertyInfo.GetCustomAttribute<ConfigPropertyAttribute>(true) == null)
            {
                throw new ArgumentException($"{propertyInfo} property doesn't have attribute ConfigProperty");
            }

            var jsonPropertyAttribute = propertyInfo.GetCustomAttribute<ConfigJsonPropertyAttribute>();
            if (jsonPropertyAttribute == null)
            {
                throw new ArgumentException("Property info is of unknown type");
            }

            return Create(new JsonConfigPropertyFiller());
        }

        private ConfigPropertyFiller Create(ConfigPropertyFiller configPropertyFiller)
        {
            configPropertyFiller.ConfigApplier = ConfigApplier;
            configPropertyFiller.Parser = Parser;
            return configPropertyFiller;
        }
    }
}