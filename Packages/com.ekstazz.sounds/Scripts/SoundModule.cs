namespace Ekstazz.Sounds
{
    using Core.Modules;
    using Zenject;
    using Zenject.Extensions.Commands;

    [AutoInstalledModule]
    public class SoundModule : ModuleInstaller
    {
        public override string Name => "Ekstazz.Sound";

        public override IModuleInitializer ModuleInitializer => new Initializer();

        public override void InstallBindings()
        {
            Container.Bind<IVibrations>().To<Vibrations>().AsSingle();
            Container.BindInterfacesTo<Sounds>().AsSingle();
            Container.Bind<SoundsView>().FromComponentInNewPrefabResource("Sounds").AsSingle();
            Container.Bind<SoundSettings>().FromResourceSettings();

            Container.DeclareSignal<SettingsChanged>();
        }

        private class Initializer : IModuleInitializer
        {
            public void Prepare()
            {
            }
        }
    }
}
