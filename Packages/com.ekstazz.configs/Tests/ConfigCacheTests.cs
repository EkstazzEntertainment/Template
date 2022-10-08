namespace Tests
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using Ekstazz.Configs;
    using Ekstazz.Configs.Cache;
    using NUnit.Framework;
    using UnityEngine;
    using UnityEngine.TestTools;

    public class ConfigCacheTests
    {
        private static string TestConfig => "{data: override, _meta: {type: \"every_time\", priority: 1}}";
        private static string TestVersionConfig => "{data: override, _meta: {type: \"single_version\"}}";
        private static string TestForeverConfig => "{data: override, _meta: {type: \"forever\"}}";
        private static string TestConcreteConfig => "{data: override, _meta: {type: \"concrete_version\", appVersion: \"***\"}}";
        private static string TestNoMetaConfig => "{data: override}";
        private static string TestInvalidMetaConfig => "{data: override, _meta: }";
        private static string TestInvalidConfig => "{data override}";
        private static string TestInvalidConfigValidMeta => "{data override, _meta: {type: \"every_time\"}";
        private static string TestInvalidMetaWithConcreteVersion => "{data override, _meta: {type: \"concrete_version\"}";
        
        
        private ConfigCache cache;
        private DummyConfigProvider provider;
        private VersionProvider versionProvider;
        
        [SetUp]
        public void SetUp()
        {
            provider = new DummyConfigProvider();
            versionProvider = new VersionProvider();
            versionProvider.Initialize();
            cache = new ConfigCache()
            {
                MetaExtractor = new MetaExtractor(),
                VersionProvider = versionProvider,
                Provider = provider
            };
            cache.Initialize();
            
            var config = "{data: from_cache}";
            cache.Serialize(new CacheSave()
            {
                configs = new Dictionary<string, Config>()
                {
                    {"forever-with-high-priority", new Config(config, new ConfigMeta(){type = ConfigMetaType.Forever, priority = 5})},
                    {"forever", new Config(config, new ConfigMeta() {type = ConfigMetaType.Forever})},
                    {"single", new Config(config, new ConfigMeta(){type = ConfigMetaType.Version})},
                    {"every-low-priority", new Config(config, new ConfigMeta() {type = ConfigMetaType.Every})},
                    {"every", new Config(config, new ConfigMeta() {type = ConfigMetaType.Every, priority = 1})}
                }
            });
        }
        
        [Test]
        public void _01ReplaceByPriority()
        {
            provider.SetConfig(TestConfig);
            var config = cache.GetOrUpdateConfig("every-low-priority", false);
            Assert.That(config, Does.Contain("data: override"));
        }
        
        [Test]
        public void _02ReplaceByNewVersion()
        {
            PlayerPrefs.DeleteAll();
            versionProvider.Initialize();
            provider.SetConfig(TestVersionConfig);
            var config = cache.GetOrUpdateConfig("single", false);
            Assert.That(config, Does.Contain("data: override"));
        }
        
        [Test]
        public void _03ReplaceByType()
        {
            provider.SetConfig(TestConfig);
            var config = cache.GetOrUpdateConfig("forever", false);
            Assert.That(config, Does.Contain("data: override"));
        }
        
        [Test]
        public void _04DoNotReplace()
        {
            provider.SetConfig(TestForeverConfig);
            var config = cache.GetOrUpdateConfig("forever", false);
            Assert.That(config, Does.Contain("data: from_cache"));
        }

        [Test]
        public void _05DoNotReplace_IfCachedPriorityIsHigher()
        {
            provider.SetConfig(TestConfig);
            var config = cache.GetOrUpdateConfig("forever-with-high-priority", false);
            Assert.That(config, Does.Contain("data: from_cache"));
        }
        
        [Test]
        public void _06ReplaceByConcreteVersion()
        {
            var concreteConfig = TestConcreteConfig;
            concreteConfig = concreteConfig.Replace("***", versionProvider.CurrentVersion.ToString());
            provider.SetConfig(concreteConfig);
            var config = cache.GetOrUpdateConfig("every-low-priority", false);
            Assert.That(config, Does.Contain("data: override"));
        }
        
        [Test]
        public void _07ReplaceEvery()
        {
            provider.SetConfig(TestConfig);
            var config = cache.GetOrUpdateConfig("every", false);
            Assert.That(config, Does.Contain("data: override"));
        }
        
        [Test]
        public void _08NoMeta()
        {
            provider.SetConfig(TestNoMetaConfig);
            var config = cache.GetOrUpdateConfig("forever", false);
            Assert.That(config, Does.Contain("data: from_cache"));
        }
        
        [Test]
        public void _09InvalidMeta()
        {
            provider.SetConfig(TestInvalidMetaConfig);
            var config = cache.GetOrUpdateConfig("forever", false);
            LogAssert.Expect(LogType.Error, new Regex(".*"));
            LogAssert.Expect(LogType.Exception, new Regex(".*"));
            Assert.That(config, Does.Contain("data: from_cache"));
        }
        
        [Test]
        public void _10InvalidConfig()
        {
            provider.SetConfig(TestInvalidConfig);
            var config = cache.GetOrUpdateConfig("forever", false);
            Assert.That(config, Does.Contain("data: from_cache"));
        }
        
        [Test]
        public void _11InvalidConfigValidMeta()
        {
            provider.SetConfig(TestInvalidConfigValidMeta);
            var config = cache.GetOrUpdateConfig("forever", false);
            Assert.That(config, Does.Contain("data override"));
        }
        
        [Test]
        public void _12InvalidConcreteVersionMetaFromProvider()
        {
            provider.SetConfig(TestInvalidMetaWithConcreteVersion);
            var config = cache.GetOrUpdateConfig("forever", false);
            Assert.That(config, Does.Contain("data: from_cache"));
        }
        
        [Test]
        public void _13ConcreteVersionWrongVersion()
        {
            var concreteConfig = TestConcreteConfig;
            var version = versionProvider.CurrentVersion;
            version = new AppVersion(version.major - 1, version.minor, version.build);
            concreteConfig = concreteConfig.Replace("***", version.ToString());
            provider.SetConfig(concreteConfig);
            provider.SetConfig(TestInvalidMetaWithConcreteVersion);
            var config = cache.GetOrUpdateConfig("forever", false);
            Assert.That(config, Does.Contain("data: from_cache"));
        }
        
        [Test]
        public void _14SingleVersionOldVersion()
        {
            provider.SetConfig(TestVersionConfig);
            var config = cache.GetOrUpdateConfig("single", false);
            Assert.That(config, Does.Contain("data: from_cache"));
        }
        
        [Test]
        public void _15SingleVersionForeverOverride()
        {
            provider.SetConfig(TestVersionConfig);
            var config = cache.GetOrUpdateConfig("forever", false);
            Assert.That(config, Does.Contain("data: override"));
        }
        
        [Test]
        public void _16NoConfigProvided()
        {
            var config = cache.GetOrUpdateConfig("forever", false);
            Assert.That(config, Does.Contain("data: from_cache"));
        }
        
        [Test]
        public void _17NoConfigProvidedNoCachedConfig()
        {
            Assert.Throws<NullReferenceException>(() => cache.GetOrUpdateConfig("invalid", false));
        }
        
        [Test]
        public void _18ExceptionFromProvider()
        {
            provider.SetException();
            var config = cache.GetOrUpdateConfig("forever", false);
            LogAssert.Expect(LogType.Error, new Regex(".*"));
            LogAssert.Expect(LogType.Exception, new Regex(".*"));
            Assert.That(config, Does.Contain("data: from_cache"));
        }
        
        private class DummyConfigProvider : IConfigProvider
        {
            private string config;
            private bool shouldThrowException;
            
            public string GetConfigOf(string key)
            {
                return GetConfig();
            }
            
            public string GetMultiConfigOf(string key)
            {
                return GetConfig();
            }
            
            private string GetConfig()
            {
                if (shouldThrowException)
                {
                    throw new Exception();
                }
                return config;
            }
            
            public void SetConfig(string config)
            {
                this.config = config;
            }
            
            public void SetException()
            {
                shouldThrowException = true;
            }
        }
    }
}