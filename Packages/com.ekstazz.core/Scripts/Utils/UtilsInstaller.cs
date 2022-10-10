namespace Ekstazz.Utils
{
    using Core.Modules;
    using Coroutine;

    
    [AutoInstalledModule]
    public class UtilsInstaller: ModuleInstaller
    {
        public override string Name => "Ekstazz.Utils";

        public override void InstallBindings()
        {
            Container.BindInterfacesTo<CoroutineProvider>().FromNewComponentOnNewGameObject().AsSingle();
            Container.BindInterfacesTo<ApplicationEventsTracker>().FromNewComponentOnNewGameObject().AsSingle();
        }
    }
}