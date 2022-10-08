namespace Ekstazz.Ui
{
    using Core.Modules;

    [AutoInstalledModule]
    public class UiInstaller : ModuleInstaller
    {
        public override string Name => "Ekstazz.Ui";

        public override void InstallBindings()
        {
            Container.Bind<UiBuilder>().To<UiBuilder>().AsSingle();
        }
    }
}
