namespace Ekstazz.Core.Modules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class ModuleSearcher
    {
        public List<IModuleInstaller> FindActiveModules()
        {
            return FindActiveInCurrentBuild()
                .Where(ActiveInCurrentPlatform)
                .ToList();

            bool ActiveInCurrentPlatform(IModuleInstaller installer)
            {
#if UNITY_EDITOR
                var platformType = PlatformType.Editor;
#elif UNITY_IOS
                var platformType = PlatformType.IOS;
#elif UNITY_ANDROID
                var platformType = PlatformType.Android;
#endif
                return (installer.SupportedPlatformType & platformType) != 0;
            }
        }

        public List<IModuleInstaller> FindActiveModulesInBuild()
        {
            return FindActiveInCurrentBuild()
                .Where(ActiveInCurrentPlatform)
                .ToList();

            bool ActiveInCurrentPlatform(IModuleInstaller installer)
            {
#if UNITY_IOS
                var platformType = PlatformType.IOS;
#elif UNITY_ANDROID
                var platformType = PlatformType.Android;
#else
                var platformType = PlatformType.Mobile;
#endif
                return (installer.SupportedPlatformType & platformType) != 0;
            }
        }

        private IEnumerable<IModuleInstaller> FindActiveInCurrentBuild()
        {
            return FindAllModules()
                .Where(ActiveInCurrentBuild);

            bool ActiveInCurrentBuild(IModuleInstaller installer)
            {
#if DEBUG
                var buildType = BuildType.Debug;
#else
                var buildType = BuildType.Release;
#endif
                return (installer.SupportedBuildType & buildType) != 0;
            }
        }

        public List<IModuleInstaller> FindAllModules()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .Where(NeedSearchForInstallers)
                .SelectMany(assembly => assembly.GetExportedTypes())
                .Where(type => type.GetCustomAttribute(typeof(AutoInstalledModuleAttribute)) != null)
                .Where(type => typeof(IModuleInstaller).IsAssignableFrom(type))
                .Select(type => (IModuleInstaller)Activator.CreateInstance(type))
                .OrderBy(installer => installer.Priority)
                .ToList();

            bool NeedSearchForInstallers(Assembly assembly)
            {
                var name = assembly.FullName;
                return name.Contains("Ekstazz") || name.Contains("Ekstazz") || name.Contains("Game.");
            }
        }
    }
}
