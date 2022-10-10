namespace Tests
{
    using Ekstazz.Configs.Cache;
    using NUnit.Framework;

    
    public class AppVersionTests
    {
        [Test]
        public void _01Equal()
        {
            var v1 = new AppVersion(1, 1, 1);
            var v2 = new AppVersion(1, 1, 1);
            Assert.That(v1, Is.EqualTo(v2));
        }
        
        [Test]
        public void _02NotEqualMajor()
        {
            var v1 = new AppVersion(1, 1, 1);
            var v2 = new AppVersion(0, 1, 1);
            Assert.That(v1, Is.Not.EqualTo(v2));
        }
        
        [Test]
        public void _03NotEqualMinor()
        {
            var v1 = new AppVersion(1, 1, 1);
            var v2 = new AppVersion(1, 0, 1);
            Assert.That(v1, Is.Not.EqualTo(v2));
        }
        
        [Test]
        public void _04NotEqualBuild()
        {
            var v1 = new AppVersion(1, 1, 1);
            var v2 = new AppVersion(1, 1, 0);
            Assert.That(v1, Is.Not.EqualTo(v2));
        }
        
        [Test]
        public void _05GreaterMajor()
        {
            var v1 = new AppVersion(2, 1, 1);
            var v2 = new AppVersion(1, 1, 1);
            Assert.That(v1, Is.GreaterThan(v2));
        }
        
        [Test]
        public void _06GreaterMinor()
        {
            var v1 = new AppVersion(1, 2, 1);
            var v2 = new AppVersion(1, 1, 1);
            Assert.That(v1, Is.GreaterThan(v2));
        }
        
        [Test]
        public void _07GreaterBuild()
        {
            var v1 = new AppVersion(1, 1, 2);
            var v2 = new AppVersion(1, 1, 1);
            Assert.That(v1, Is.GreaterThan(v2));
        }
        
        [Test]
        public void _08LessMajorGreaterMinor()
        {
            var v1 = new AppVersion(0, 2, 1);
            var v2 = new AppVersion(1, 1, 1);
            Assert.That(v1, Is.LessThan(v2));
        }
        
        [Test]
        public void _09ToString()
        {
            var v = new AppVersion(1, 1, 1);
            Assert.That(v.ToString(), Is.EqualTo("1.1.1"));
        }
    }
}