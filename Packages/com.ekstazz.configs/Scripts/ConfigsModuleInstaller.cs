namespace Ekstazz.Configs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Cache;
    using Ekstazz.Core;
    using Ekstazz.Core.Modules;
    using Settings;
    using UnityEngine;
    using Zenject;

    
    [AutoInstalledModule]
    public class ConfigsModuleInstaller : ModuleInstaller
    {
        public override IModuleInitializer ModuleInitializer => new Initializer();
        public override Priority Priority => Priority.High;
        public override string Name => "Ekstazz.Configs";

        public override void InstallBindings()
        {
            var (main, backup) = FindConfigServiceProviders();

            Container.Bind<CacheIoWorker>().To<CacheIoWorker>().AsSingle();
            Container.Bind<IMetaExtractor>().To<MetaExtractor>().AsSingle();
            Container.Bind<IVersionProvider>().To<VersionProvider>().AsSingle();
            Container.Bind<ICacheSaver>().To<CacheSaver>().AsSingle();

            var localFetcher = new ConfigFetcher {ServiceWrapper = backup.CreateWrapper()};
            var remoteFetcher = new ConfigFetcher {ServiceWrapper = main.CreateWrapper()};

            var configCache = new ConfigCache() {Provider = remoteFetcher};
            Container.Bind(typeof(IConfigCache), typeof(IInitializable)).FromInstance(configCache);
            Container.QueueForInject(configCache);

            var settings = CacheSettings.Load();
            var applier = new BackedUpConfigApplier
            {
                Primary = main.Name != backup.Name && settings.IsCacheEnabled
                    ? (IConfigProvider) configCache
                    : remoteFetcher,
                Secondary = localFetcher,
            };

            Container.Bind<IConfigProvider>().FromInstance(localFetcher);
            Container.Bind<IConfigFetcher>()
                .WithId(FetcherOrigin.Remote)
                .FromInstance(remoteFetcher);
            Container.Bind<IConfigFetcher>()
                .WithId(FetcherOrigin.Local)
                .FromInstance(localFetcher);

            var postProcessorsFactory = new PostProcessorsFactory();
            var configPropertyFillerFactory = new ConfigPropertyFillerFactory()
            {
                ConfigApplier = applier,
                Parser = new Parser(),
            };

            var configTypeFiller = new ConfigTypeFiller()
            {
                FillerFactory = configPropertyFillerFactory,
                PostProcessorsFactory = postProcessorsFactory,
            };

            Container.Bind<ConfigTypeFiller>().FromInstance(configTypeFiller);
            Container.QueueForInject(configTypeFiller);

            var configurableProvider = new ConfigurableProvider();
            Container.Bind<IConfigurableProvider>().FromInstance(configurableProvider);
            Container.QueueForInject(configurableProvider);

            BindAutoConfigurables(configurableProvider);
        }

        private static (IConfigServiceProvider main, IConfigServiceProvider backup) FindConfigServiceProviders()
        {
            var serviceProviders = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => typeof(IConfigServiceProvider).IsAssignableFrom(type))
                .Where(type => type != typeof(IConfigServiceProvider))
                .Select(Activator.CreateInstance)
                .OfType<IConfigServiceProvider>()
                .ToArray();

            if (!serviceProviders.Any())
            {
                throw new Exception(
                    "No service providers for Ekstazz.Configs. There should be at lease Resource service provider.");
            }

            var providerWithMinPriority = serviceProviders.OrderBy(provider => provider.Priority).First();
            var providerWithMaxPriority = serviceProviders.OrderByDescending(provider => provider.Priority).First();

            Debug.Log(
                $"RC: Using {providerWithMaxPriority.Name} as main configs provider, {providerWithMinPriority.Name} as backup.");

            return (providerWithMaxPriority, providerWithMinPriority);
        }

        private void BindAutoConfigurables(ConfigurableProvider configurableProvider)
        {
            var autoConfigurableObjects = AppDomain.CurrentDomain.GetAssemblies()
                .Where(NeedSearchForInstallers)
                .SelectMany(assembly => assembly.GetTypes())
                .Where(HasAutoConfigAttribute)
                .Select(Activator.CreateInstance);

            foreach (var obj in autoConfigurableObjects)
            {
                Container.BindInterfacesAndSelfTo(obj.GetType()).FromInstance(obj);
                Container.QueueForInject(obj);
                configurableProvider.AddObjectToConfigure(obj);
            }

            bool NeedSearchForInstallers(Assembly assembly)
            {
                var name = assembly.FullName;
                return name.Contains("Ekstazz") || name.Contains("Ekstazz") || name.Contains("Game.");
            }

            bool HasAutoConfigAttribute(Type type)
            {
                return Attribute.IsDefined(type, typeof(AutoConfigurableAttribute));
            }
        }

        protected override IEnumerable<ModuleVerificationResult> FindModuleErrors()
        {
            var settings = CacheSettings.Load();
            if (settings == null)
            {
                yield return ModuleVerificationResult.HasError(
                    "Please, add CacheSettings asset to Resources/Settings via Create menu (Ekstazz/Configs/Cache/Settings)");
            }
        }

        private class Initializer : IModuleInitializer
        {
            public void Prepare()
            {
            }
        }
    }
}