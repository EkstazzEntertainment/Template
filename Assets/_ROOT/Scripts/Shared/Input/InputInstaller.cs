namespace Ekstazz.Input
{
    using Ekstazz.Core.Modules;
    using HotKey;
    using Settings;
    using Zenject.Extensions.Commands;

    
    [AutoInstalledModule]
    public class InputInstaller : ModuleInstaller
    {
        public override string Name => "Ekstazz.Input";

        public override IModuleInitializer ModuleInitializer => new Initializer();

        
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<InputProvider>().AsSingle();
            Container.BindInterfacesTo<HotKeyDebug>().AsSingle();
            Container.BindInterfacesAndSelfTo<Joystick>().AsSingle();

            Container.BindInterfacesAndSelfTo<CustomInputSettings>().FromResourceSettings("Settings/Input");
        }

        private class Initializer : IModuleInitializer
        {
            public void Prepare()
            {
            }
        }
    }
}
