namespace Ekstazz.LevelBased.Debug
{
    using Ekstazz.Core.Modules;
    using Ekstazz.Input.HotKey;
    using Zenject;

    
    [AutoInstalledModule]
    public class LevelBasedDebugInstaller : ModuleInstaller
    {
        public override string Name => "Game.LevelBased.Debug";
        public override IModuleInitializer ModuleInitializer => new Initializer();

        
        public override void InstallBindings()
        {
            
        }

        private class Initializer : IModuleInitializer
        {
            [Inject] private IHotKeyDebug hotKeyDebug;

            public void Prepare()
            {
                hotKeyDebug.RegisterAction(new LevelFailAction());
                hotKeyDebug.RegisterAction(new LevelCompleteAction());
                hotKeyDebug.RegisterAction(new LevelRestartAction());
            }
        }
    }
}
