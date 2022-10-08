namespace Ekstazz.Configs.Cache
{
    using System;
    using Newtonsoft.Json;

    internal interface IMetaExtractor
    {
        Config SplitConfig(string config);
    }

    internal class MetaExtractor : IMetaExtractor
    {
        private const string MetaProperty = "_meta";

        public Config SplitConfig(string config)
        {
            var index = config.IndexOf(MetaProperty, StringComparison.InvariantCulture);
            return index == -1 ? new Config(config, new ConfigMeta()) : SplitConfigWithMeta(config, index);
        }

        private Config SplitConfigWithMeta(string config, int index)
        {
            var count = 0;
            var startIndex = index;
            var endIndex = index;
            for (var i = index; i < config.Length; i++)
            {
                var character = config[i];
                if (character == '{')
                {
                    if (count == 0) startIndex = i;
                    count++;
                }
                else if (character == '}')
                {
                    count--;
                    if (count == 0)
                    {
                        endIndex = i;
                        break;
                    }
                }
            }

            if (count != 0 || startIndex == endIndex)
            {
                throw new Exception($"Could not extract meta. \n{config}");
            }

            var metaString = config.Substring(startIndex, endIndex - startIndex + 1);
            var meta = DeserializeMeta(metaString);

            config = config.Remove(startIndex, endIndex - startIndex + 1);
            var hasDots = config[index + MetaProperty.Length] == ':';
            config = config.Remove(index, MetaProperty.Length + (hasDots ? 1 : 0));
            return new Config(config, meta);
        }

        private ConfigMeta DeserializeMeta(string metaString)
        {
            return JsonConvert.DeserializeObject<ConfigMeta>(metaString);
        }
    }
}