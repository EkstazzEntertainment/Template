namespace Tests
{
    using System;
    using Ekstazz.Configs.Cache;
    using NUnit.Framework;

    public class MetaExtractorTests
    {
        private IMetaExtractor extractor;
        
        [OneTimeSetUp]
        public void SetUp()
        {
            extractor = new MetaExtractor();
        }
        
        [Test]
        public void _01ExtractValidMeta()
        {
            var config = @"{
  _meta: {priority: 1}
}";
            var cachedConfig = extractor.SplitConfig(config);
            Assert.That(cachedConfig.meta.priority, Is.EqualTo(1));
        }
        
        [Test]
        public void _02ExtractNoMeta()
        {
            var config = @"{}";
            var cachedConfig = extractor.SplitConfig(config);
            Assert.That(cachedConfig.meta.priority, Is.EqualTo(0));
        }
        
        [Test]
        public void _03ExtractInvalidPriority()
        {
            var config = @"{
  _meta: {priorit: 1}
}";
            var cachedConfig = extractor.SplitConfig(config);
            Assert.That(cachedConfig.meta.priority, Is.EqualTo(0));
        }
        
        [Test]
        public void _04ExtractInvalidMeta()
        {
            var config = @"{
  _meta: }
}";
            Assert.Throws<Exception>(() => extractor.SplitConfig(config));
        }
        
        [Test]
        public void _05ExtractEmptyMeta()
        {
            var config = @"{
                _meta: 
            }";
            Assert.Throws<Exception>(() => extractor.SplitConfig(config));
        }
        
        [Test]
        public void _06ExtractFromCsv()
        {
            var config = @"test,_meta
1,{priority: 1}";
            var cachedConfig = extractor.SplitConfig(config);
            Assert.That(cachedConfig.meta.priority, Is.EqualTo(1));
        }
    }
}