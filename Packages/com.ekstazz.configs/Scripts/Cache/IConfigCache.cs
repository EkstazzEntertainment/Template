namespace Ekstazz.Configs.Cache
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using Zenject;

    
    internal interface IConfigCache
    {
        string GetOrUpdateConfig(string key, bool useMultiConfig);
        Config GetConfig(string key, bool useMultiConfig);
        void Serialize(CacheSave configs);
        CacheSave Deserialize();
    }

    
    internal class ConfigCache : IConfigCache, IConfigProvider, IInitializable
    {
        [Inject] public IMetaExtractor MetaExtractor { get; set; }
        [Inject] public IVersionProvider VersionProvider { get; set; }

        public IConfigProvider Provider { get; set; }

        private Dictionary<string, Config> configs = new Dictionary<string, Config>();
        private IConfigRule rule;

        
        public void Initialize()
        {
            var typeRule = new TypeRule(null);
            var versionRule = new NewVersionRule(VersionProvider, typeRule);
            var concreteVersionRule = new ConcreteVersionRule(VersionProvider, versionRule);
            var everyRule = new EveryTimeRule(concreteVersionRule);
            var priorityRule = new PriorityRule(everyRule);
            rule = priorityRule;
        }

        public string GetConfigOf(string key)
        {
            return GetOrUpdateConfig(key, false);
        }

        public string GetMultiConfigOf(string key)
        {
            return GetOrUpdateConfig(key, true);
        }

        public string GetOrUpdateConfig(string key, bool useMultiConfig)
        {
            var oldConfig = GetConfig(key, useMultiConfig);
            var newConfig = FetchConfig(key, useMultiConfig);
            if (newConfig == null)
            {
                return oldConfig.value;
            }

            if (rule.ShouldReplace(oldConfig.meta, newConfig.meta))
            {
                AddConfigToCache(key, newConfig);
                return newConfig.value;
            }

            return oldConfig.value;
        }

        public Config GetConfig(string key, bool useMultiConfig)
        {
            if (configs.TryGetValue(key, out var config))
            {
                return config;
            }

            config = FetchConfig(key, useMultiConfig);
            if (config != null)
            {
                AddConfigToCache(key, config);
            }

            return config;
        }

        private Config FetchConfig(string key, bool useMultiConfig)
        {
            var config = FetchConfigFromProvider(key, useMultiConfig);
            if (string.IsNullOrEmpty(config))
            {
                return null;
            }
            try
            {
                return MetaExtractor.SplitConfig(config);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to parse config with key: {key}. Please check that config has valid meta or no meta at all.\n{config}");
                Debug.LogException(e);
                return new Config(config, new ConfigMeta());
            }
        }

        private string FetchConfigFromProvider(string key, bool useMultiConfig)
        {
            try
            {
                return useMultiConfig ? Provider.GetMultiConfigOf(key) : Provider.GetConfigOf(key);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to fetch config from provider: {key}");
                Debug.LogException(e);
                return null;
            }
        }

        private void AddConfigToCache(string key, Config config)
        {
            configs[key] = config;
        }

        public void Serialize(CacheSave save)
        {
            configs = save.configs;
        }

        public CacheSave Deserialize()
        {
            return new CacheSave()
            {
                configs = configs.ToDictionary(pair => pair.Key, pair => pair.Value)
            };
        }
    }
}