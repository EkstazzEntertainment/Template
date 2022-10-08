namespace Zenject.Extensions.Commands
{
    using System;
    using System.Linq;
    using ModestTree;
    using UnityEngine;

    public static class BinderExtensions
    {
        private const string SettingsPath = "Settings";

        public static IfNotBoundBinder FromResourceSettings(this FromBinder fromBinder,
            string path = null)
        {
            path = GetSettingsPath(fromBinder, path);
            return fromBinder.FromScriptableObjectResource(path)
                .AsSingle()
                .NonLazy();
        }

        private static string GetSettingsPath(FromBinder fromBinder, string path)
        {
            return string.IsNullOrEmpty(path) ? GetDefaultPath(fromBinder) : GetPathByType(fromBinder, path);
        }

        private static string GetDefaultPath(FromBinder fromBinder)
        {
            return GetPathByType(fromBinder, SettingsPath);
        }

        private static string GetPathByType(FromBinder fromBinder, string path)
        {
            var settingsType = FindScriptableObjectType(fromBinder);
            return $"{path}/{settingsType.Name}";
        }

        private static Type FindScriptableObjectType(FromBinder fromBinder)
        {
            return fromBinder.BindInfo.ContractTypes.First(type => type.DerivesFrom(typeof(ScriptableObject)));
        }
    }
}
