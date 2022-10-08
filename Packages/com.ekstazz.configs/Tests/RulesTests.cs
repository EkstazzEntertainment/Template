namespace Tests
{
    using System.Linq;
    using Ekstazz.Configs.Cache;
    using NUnit.Framework;

    public class RulesTests
    {
        [Test]
        public void _01PriorityRuleBigger()
        {
            var rule = new PriorityRule(null);
            var oldMeta = new ConfigMeta(){priority = 0};
            var newMeta = new ConfigMeta(){priority = 1};
            Assert.True(rule.ShouldReplace(oldMeta, newMeta));
        }
        
        [Test]
        public void _02PriorityRuleLess()
        {
            var rule = new PriorityRule(null);
            var oldMeta = new ConfigMeta(){priority = 1};
            var newMeta = new ConfigMeta(){priority = 0};
            Assert.False(rule.ShouldReplace(oldMeta, newMeta));
        }
        
        [Test]
        public void _03PriorityRuleEqual()
        {
            var rule = new PriorityRule(null);
            var oldMeta = new ConfigMeta(){priority = 1};
            var newMeta = new ConfigMeta(){priority = 1};
            Assert.False(rule.ShouldReplace(oldMeta, newMeta));
        }
        
        [Test]
        public void _04VersionRuleNewVersion()
        {
            var versionProvider = new DummyVersionProvider {IsNewVersion = true};
            var rule = new NewVersionRule(versionProvider, null);
            var oldMeta = new ConfigMeta(){};
            var newMeta = new ConfigMeta(){type = ConfigMetaType.Version};
            Assert.True(rule.ShouldReplace(oldMeta, newMeta));
        }
        
        [Test]
        public void _05VersionRuleOldVersion()
        {
            var versionProvider = new DummyVersionProvider {IsNewVersion = false};
            var rule = new NewVersionRule(versionProvider, null);
            var oldMeta = new ConfigMeta(){};
            var newMeta = new ConfigMeta(){type = ConfigMetaType.Version};
            Assert.False(rule.ShouldReplace(oldMeta, newMeta));
        }
        
        [Test]
        public void _06ConcreteVersionRuleCurrentVersion()
        {
            var versionProvider = new DummyVersionProvider {CurrentVersion = new AppVersion(1, 1, 1)};
            var rule = new ConcreteVersionRule(versionProvider, null);
            var oldMeta = new ConfigMeta(){};
            var newMeta = new ConfigMeta(){appVersion = "1.1.1", type = ConfigMetaType.ConcreteVersion};
            Assert.True(rule.ShouldReplace(oldMeta, newMeta));
        }
        
        [Test]
        public void _07ConcreteVersionRuleNewVersion()
        {
            var versionProvider = new DummyVersionProvider {CurrentVersion = new AppVersion(1, 1, 1)};
            var rule = new ConcreteVersionRule(versionProvider, null);
            var oldMeta = new ConfigMeta(){};
            var newMeta = new ConfigMeta(){appVersion = "1.1.2", type = ConfigMetaType.ConcreteVersion};
            Assert.False(rule.ShouldReplace(oldMeta, newMeta));
        }
        
        [Test]
        public void _08TypeRuleForever()
        {
            var rule = new TypeRule(null);
            var forever = new ConfigMeta(){type = ConfigMetaType.Forever};
            var single = new ConfigMeta(){type = ConfigMetaType.Version};
            var every = new ConfigMeta(){type = ConfigMetaType.Every};
            Assert.True(rule.ShouldReplace(forever, single));
            Assert.True(rule.ShouldReplace(forever, every));
        }
        
        [Test]
        public void _09TypeRuleSingle()
        {
            var rule = new TypeRule(null);
            var forever = new ConfigMeta(){type = ConfigMetaType.Forever};
            var single = new ConfigMeta(){type = ConfigMetaType.Version};
            var every = new ConfigMeta(){type = ConfigMetaType.Every};
            Assert.False(rule.ShouldReplace(single, forever));
            Assert.True(rule.ShouldReplace(single, every));
        }
        
        [Test]
        public void _10TypeRuleEvery()
        {
            var rule = new TypeRule(null);
            var forever = new ConfigMeta(){type = ConfigMetaType.Forever};
            var single = new ConfigMeta(){type = ConfigMetaType.Version};
            var every = new ConfigMeta(){type = ConfigMetaType.Every};
            Assert.False(rule.ShouldReplace(every, forever));
            Assert.False(rule.ShouldReplace(every, single));
        }
        
        [Test]
        public void _11EveryRule()
        {
            var rule = new EveryTimeRule(null);
            var oldConfig = new ConfigMeta(){type = ConfigMetaType.Every};
            var newConfig = new ConfigMeta(){type = ConfigMetaType.Every};
            Assert.True(rule.ShouldReplace(oldConfig, newConfig));
        }
        
        private class DummyVersionProvider : IVersionProvider
        {
            public AppVersion CurrentVersion { get; set; }
            
            public bool IsNewVersion { get; set; }
            
            public AppVersion ParseVersionString(string versionString)
            {
                var parts = versionString.Split('.').Select(int.Parse).ToArray();
                return new AppVersion(parts[0], parts[1], parts[2]);
            }
        }
    }
}