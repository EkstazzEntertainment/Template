namespace Ekstazz.Core
{
    using Modules;
    using Zenject;

    
    [AutoInstalledModule]
    public class CoreInstaller : ModuleInstaller
    {
        public override Priority Priority => Priority.Highest;
        public override string Name => "Ekstazz.Core";

        
        public override void InstallBindings()
        {
            Container.DeclareSignal<StartApp>();
        }
    }
}