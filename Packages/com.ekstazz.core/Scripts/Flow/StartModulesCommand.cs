namespace Ekstazz.Core
{
    using System;
    using System.Threading.Tasks;
    using Modules;
    using UnityEngine;
    using UnityEngine.Profiling;
    using Zenject;
    using Zenject.Extensions.Commands;

    
    public class StartModulesCommand : Command
    {
        [Inject] public ContextInstallersHolder ContextInstallersHolder { get; set; }
        [Inject] public DiContainer DiContainer { get; set; }

        
        public override async Task Execute()
        {
            Profiler.BeginSample("Starting modules");
            foreach (var moduleInitializer in ContextInstallersHolder.ModuleInitializers)
            {
                Profiler.BeginSample($"Starting module logic {moduleInitializer.GetType()}");
                try
                {
                    DiContainer.Inject(moduleInitializer);
                    moduleInitializer.Prepare();
                }
                catch (Exception)
                {
                    Debug.LogError($"<color=red>Error occured while starting game modules :( See the next error log for details</color>");
                    throw;
                }

                Profiler.EndSample();
            }

            Profiler.EndSample();
        }
    }
}
