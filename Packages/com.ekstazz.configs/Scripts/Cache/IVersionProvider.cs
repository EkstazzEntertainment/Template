namespace Ekstazz.Configs.Cache
{
    using System.Linq;
    using UnityEngine;
    using Zenject;

    
    internal interface IVersionProvider
    {
        AppVersion CurrentVersion { get; }
        bool IsNewVersion { get; }
        AppVersion ParseVersionString(string versionString);
    }

    
    internal class VersionProvider : IVersionProvider, IInitializable
    {
        private const string Key = "_version";

        public AppVersion CurrentVersion { get; private set; }
        public bool IsNewVersion { get; private set; }

        
        public void Initialize()
        {
            CheckApplicationVersion();
            PlayerPrefs.SetString(Key, Application.version);
            CurrentVersion = ParseVersionString(Application.version);
        }

        private void CheckApplicationVersion()
        {
            if (PlayerPrefs.HasKey(Key))
            {
                var versionString = PlayerPrefs.GetString(Key);
                var version = ParseVersionString(versionString);
                IsNewVersion = CurrentVersion > version;
            }
            else
            {
                IsNewVersion = true;
            }
        }

        public AppVersion ParseVersionString(string versionString)
        {
            if (string.IsNullOrEmpty(versionString))
            {
                return new AppVersion();
            }

            var parts = versionString
                .Split('.')
                .Select(s => int.TryParse(s, out var n) ? n : 0)
                .ToArray();
            var major = parts.Length > 0 ? parts[0] : 0;
            var minor = parts.Length > 1 ? parts[1] : 0;
            var build = parts.Length > 2 ? parts[2] : 0;
            
            return new AppVersion(major, minor, build);
        }
    }
}