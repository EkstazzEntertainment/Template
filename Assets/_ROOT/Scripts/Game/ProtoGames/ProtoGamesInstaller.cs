namespace Ekstazz.ProtoGames
{
    using Currency;
    using Ekstazz.Core;
    using Ekstazz.Core.Modules;
    using Ekstazz.Currencies;
    using Ekstazz.Debug.DebugOptions;
    using Ekstazz.LevelBased.SceneLoading;
    using Ekstazz.Sounds;
    using Ekstazz.Tools;
    using Game.Flow;
    using Game1;
    using Game2;
    using LevelBased.Flow.Signals;
    using Settings;
    using Settings.Camera;
    using Sound;
    using Zenject;
    using Zenject.Extensions.Commands;


    [AutoInstalledModule]
    public class ProtoGamesInstaller : ModuleInstaller
    {
        public override string Name => "Game.ProtoGames";
        public override IModuleInitializer ModuleInitializer => new Initializer();
        public override Priority Priority => Priority.Low;

        private DiContainer game1Container;
        private DiContainer game2Container;
        
          
        public override async void InstallBindings()
        {
            Container.Bind(typeof(GameSounds), typeof(ProtoGameSounds), typeof(IInitializable))
                .To<ProtoGameSounds>().AsSingle();
            Container.Bind<PauseTracker>().FromNewComponentOnNewGameObject().AsSingle();
            
            Container.Bind<LevelGameProvider>().AsSingle();
            Container.BindInterfacesTo<LevelParentProvider>().AsSingle();

            InstallSignals();
            InstallResources();
            InstallProviders();
            InstallControllersAndHandlers();
                        
            InstallDebugComponents();

            Container.BindSignalToCommand<GameSceneLoaded>()
                .To<WaitFrameCommand>()
                .To<WaitFrameCommand>() 
                .To<WaitFrameCommand>()
                .To<SignalWrapper<SwitchingToHome>>()
                .To<SignalWrapper<LoadLevel>>();
            
            game1Container = CreateLevelGameSubContainer<Game1Installer>();
            game2Container = CreateLevelGameSubContainer<Game2Installer>();
            
            ResolveLevelGameProvider();
        }
        
        private DiContainer CreateLevelGameSubContainer<T>() where T : Installer<T>
        {
            var subContainer = Container.CreateSubContainer();
            subContainer.Instantiate<T>().InstallBindings();
            BindLevelGameFrom(subContainer);
            BindToKernel(subContainer);
            return subContainer;
        }
        
        private void BindLevelGameFrom(DiContainer subContainer)
        {
            var levelGame = subContainer.Resolve<LevelGame>();

            Container.BindInterfacesAndSelfTo<LevelGame>()
                .FromInstance(levelGame)
                .AsCached()
                .NonLazy();
        }

        private void ResolveLevelGameProvider()
        {
            var levelGames = Container.ResolveAll<LevelGame>(); 
            var levelGameProvider = Container.Resolve<LevelGameProvider>();
            levelGameProvider.AddLevelGames(levelGames);
        }

        private void BindToKernel(DiContainer subContainer)
        {
            var kernel = subContainer.Resolve<Kernel>();
            Container.BindInterfacesTo<Kernel>()
                .FromInstance(kernel);
        }

        private void InstallSignals()
        {
            Container.DeclareSignal<RestartActivityTracker>();
            Container.DeclareSignal<TrackPreviousSessionLastLevelStatus>();
            Container.DeclareSignal<SubContainersInjected>();
        }

        private void InstallResources()
        {
            Container.Bind<PrefabSettings>().FromResourceSettings();
            Container.Bind<CameraDefaultInfosSettings>().FromResourceSettings("Settings/Common");
        }
        
        private void InstallProviders()
        {
        }

        private void InstallControllersAndHandlers()
        {
        }

        private void InstallDebugComponents()
        {
        }

        private class Initializer : IModuleInitializer
        {
            [Inject] public MoneyFactory MoneyFactory { get; set; }
            [Inject] public DebugOptionsRouter DebugOptionsRouter { get; set; }


            public void Prepare()
            {
                MoneyFactory.Register<Coin>();
                PrepareDebugComponents();
            }
            
            private void PrepareDebugComponents()
            {
                
            }
        }
    }
}
