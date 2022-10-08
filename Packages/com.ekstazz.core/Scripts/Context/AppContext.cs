namespace Ekstazz.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Modules;
    using UnityEngine;
    using UnityEngine.Profiling;
    using Zenject;

    public class AppContext : MonoInstaller
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            var modules = FindModules();
            VerifyModules(modules);
            InstallModules(modules);
        }

        private List<IModuleInstaller> FindModules()
        {
            Profiler.BeginSample("Searching for modules");
            var moduleSearches = new ModuleSearcher();
            var moduleInstallers = moduleSearches.FindActiveModules();
            Profiler.EndSample();

            var activeModules = string.Join(", ", moduleInstallers.Select(installer => installer.Name));
            Debug.Log($"<color=green>Active modules ordered by priority: {activeModules}</color>");

            return moduleInstallers;
        }

        private void VerifyModules(List<IModuleInstaller> modules)
        {
            Profiler.BeginSample("Verifying modules");
            var modulesVerifier = new ModulesVerifier();
            modulesVerifier.Verify(modules);
            modulesVerifier.PrintMessages();
            Profiler.EndSample();
        }

        private void InstallModules(List<IModuleInstaller> modules)
        {
            Container.Bind<ContextInstallersHolder>().FromInstance(new ContextInstallersHolder(modules));

            Profiler.BeginSample("Installing modules");
            foreach (var module in modules)
            {
                Profiler.BeginSample($"Installing {module.Name}");
                var contextInstaller = module.ContextInstaller;
                try
                {
                    if (contextInstaller is null)
                    {
                        throw new Exception($"Context installer in {module.Name} is null, not installing");
                    }

                    InstallModule(contextInstaller);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error installing {module.Name} module. Exception details:\r\n{e}");
                }

                Profiler.EndSample();
            }

            Profiler.EndSample();

            void InstallModule(Installer contextInstaller)
            {
                Container.Inject(contextInstaller);
                contextInstaller.InstallBindings();
            }
        }
    }
}
