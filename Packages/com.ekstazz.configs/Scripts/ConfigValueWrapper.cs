namespace Ekstazz.Configs
{
    using System;

    public interface IConfigValueWrapper
    {
        string StringValue { get; }
    }

    public class SimpleValueWrapper : IConfigValueWrapper 
    {
        public string StringValue { get; }

        public SimpleValueWrapper(string stringValue)
        {
            StringValue = stringValue;
        }
        
        public SimpleValueWrapper(string configValue, Func<string, string> stringPreprocessor)
        {
            StringValue =  stringPreprocessor(configValue);
        }
    }


    public class ConfigValueWrapper : IConfigValueWrapper
    {
//        public ConfigValueWrapper(ConfigValue configValue, Func<string,string> stringPreprocessor)
//        {
//            StringValue = stringPreprocessor(configValue.StringValue);
//        }
//
//        public ConfigValueWrapper(ConfigValue configValue) : this(configValue, x=>x)
//        {
//        }

        public ConfigValueWrapper(string value)
        {
            StringValue = value;
        }

        public ConfigValueWrapper(string configValue, Func<string, string> stringPreprocessor)
        {
            StringValue =  stringPreprocessor(configValue);
        }

        public string StringValue { get; private set; }
    }
}