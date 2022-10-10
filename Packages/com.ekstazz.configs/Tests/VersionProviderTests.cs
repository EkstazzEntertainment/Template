namespace Tests
{
    using System.Linq;
    using Ekstazz.Configs.Cache;
    using NUnit.Framework;
    using UnityEngine;

    
    public class VersionProviderTests
    {
        private VersionProvider versionProvider;
        
        
        [OneTimeSetUp]
        public void SetUp()
        {
            versionProvider = new VersionProvider();
        }
        
        [Test]
        public void _01CurrentVersion()
        {
            versionProvider.Initialize();
            var version = versionProvider.CurrentVersion;
            var parts = Application.version.Split('.').Select(int.Parse).ToArray();
            var major = parts[0];
            var minor = parts[1];
            Assert.That(version.major, Is.EqualTo(major));
            Assert.That(version.minor, Is.EqualTo(minor));
        }
        
        [Test]
        public void _02NewVersionFirstLaunch()
        {
            PlayerPrefs.DeleteAll();
            versionProvider.Initialize();
            Assert.True(versionProvider.IsNewVersion);
        }
        
        [Test]
        public void _03ParseVersionString()
        {
            var version = versionProvider.ParseVersionString("1.1.1");
            var expected = new AppVersion(1, 1, 1);
            Assert.That(version, Is.EqualTo(expected));
        }
        
        [Test]
        public void _04ParseVersionStringMissingBuild()
        {
            var version = versionProvider.ParseVersionString("1.1");
            var expected = new AppVersion(1, 1, 0);
            Assert.That(version, Is.EqualTo(expected));
        }
        
        [Test]
        public void _05ParseVersionStringWrongDelimiter()
        {
            var version = versionProvider.ParseVersionString("1,1,1");
            var expected = new AppVersion(0, 0, 0);
            Assert.That(version, Is.EqualTo(expected));
        }
        
        [Test]
        public void _06ParseVersionStringNotNumber()
        {
            var version = versionProvider.ParseVersionString("a.b.c");
            var expected = new AppVersion(0, 0, 0);
            Assert.That(version, Is.EqualTo(expected));
        }
    }
}