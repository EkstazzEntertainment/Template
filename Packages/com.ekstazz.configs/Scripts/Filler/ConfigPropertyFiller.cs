namespace Ekstazz.Configs
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    
    
    internal abstract class ConfigPropertyFiller
    {
        internal IConfigParser Parser { get; set; }
        internal BackedUpConfigApplier ConfigApplier { get; set; }

        
        public virtual void Fill(object obj, PropertyInfo propertyInfo, string propertykey, bool useMultiConfig = false)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            if (propertyInfo == null)
            {
                throw new ArgumentNullException(nameof(propertyInfo));
            }

            ThrowIfTypeOfPropertyIsNotCorrect(propertyInfo);

            try
            {
                ConfigApplier.ApplyConfigOf(propertykey, value => FillImpl(obj, propertyInfo, value), useMultiConfig);
            }
            catch (KeyNotFoundException e)
            {
                throw new Exception($"No such key in ConfigFetcher: {propertykey}. The property will be unassigned.", e);
            }
            catch (Exception e)
            {
                throw new Exception($"Failed to fill property with key \"{propertykey}\". The property will be unassigned.", e);
            }
        }

        protected abstract void ThrowIfTypeOfPropertyIsNotCorrect(PropertyInfo propertyInfo);

        protected void FillImpl(object obj, PropertyInfo propertyInfo, string value)
        {
            var configValue = value;
            var genericMethod = GetParserMethodForTypeOf(propertyInfo);

            var result = genericMethod.Invoke(Parser, new object[] {configValue});
            propertyInfo.SetValue(obj, result);
        }

        protected abstract MethodInfo GetParserMethodForTypeOf(PropertyInfo propertyInfo);
    }
}