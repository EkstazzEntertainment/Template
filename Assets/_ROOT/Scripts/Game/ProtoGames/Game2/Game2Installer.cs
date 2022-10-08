namespace Ekstazz.ProtoGames.Game2
{
    using Zenject;
    using Character;
    using Ekstazz.LevelBased.PrefabLevels;
    using Ekstazz.Ui;
    using Flow;
    using Flow.Signals;
    using Game.Flow;
    using Input;
    using Level;
    using LevelBased.Flow;
    using LevelBased.Flow.Signals;
    using LevelBased.Logic;
    using ProtoGames.Character;
    using ProtoGames.Flow;
    using ProtoGames.Level;
    using ProtoGames.Level.Logic;
    using Settings;
    using Ui;
    using UnityEngine;
    using Zenject.Extensions.Commands;


    public class Game2Installer : Installer<Game2Installer>
    {
        private string settingsPath = "Game2/Settings";


        public override void InstallBindings()
        {
            Container.BindFactory<Area, Transform, Area, PrefabLevelFactory>()
                .FromFactory<PrefabLevelFactory>();

            Container.Bind<LevelGame>().To<Game2LevelGame>().AsSingle();
            Container.Bind(typeof(ILevelViewProvider<Game2LevelView>), typeof(ILevelViewFinder))
                .To<LevelViewProvider<Game2LevelView>>().AsSingle();

            Container.Bind<PrefabLevelBuilder>().To<Game2PrefabBuilder>().AsSingle();
            Container.Bind<UiBuilder>().To<Game2UiBuilder>().AsSingle();
            Container.Bind<Kernel>().AsSingle();

            InstallSignals();
            InstallResources();
            InstallFactories();
            InstallProviders();
            InstallControllersAndHandlers();

            InstallLevelGameFlow();
        }

        private void InstallSignals()
        {
            Container.DeclareSignal<Game2ReadyToStart>();
            Container.DeclareSignal<Game2Started>();
            Container.DeclareSignal<Game2Completing>();
            Container.DeclareSignal<Game2Completed>();
            Container.DeclareSignal<Game2Failing>();
            Container.DeclareSignal<Game2Failed>();
            Container.DeclareSignal<Game2Ending>();
            Container.DeclareSignal<Game2Ended>();
            Container.DeclareSignal<Game2Restarting>();

            Container.DeclareSignal<ModelChangedInGame2>();
        }

        private void InstallResources()
        {
            Container.Bind<Game2SettingsForTestingPurposes>().FromResourceSettings(settingsPath);
        }

        private void InstallFactories()
        {
            Container.BindFactory<Object, CharacterView, Factories.CharacterFactory>()
                .FromFactory<PrefabFactory<CharacterView>>();
        }

        private void InstallProviders()
        {
            Container.BindInterfacesAndSelfTo<Game2Input>().AsSingle();
            Container.BindInterfacesTo<Game2WorldThemeProvider>().AsSingle();
        }

        private void InstallControllersAndHandlers()
        {
            Container.BindInterfacesAndSelfTo<LevelStyleAdjuster<Game2ReadyToStart>>().AsSingle();
            Container.BindInterfacesAndSelfTo<Game2CharacterMovement>().AsSingle();
            Container.BindInterfacesAndSelfTo<Game2CharacterRotation>().AsSingle();
            Container.BindInterfacesAndSelfTo<Game2CharacterAnimator>().AsSingle();
            Container.BindInterfacesAndSelfTo<Game2LevelCameraAdjuster>().AsSingle();
        }

        private void InstallLevelGameFlow()
        {
            Container.BindSignalToCommand<Game2ReadyToStart>()
                .To<WaitFrameCommand>()
                .To<SpawnGame2CharacterCommand>()
                .To<SwitchToGame2HomeCamera>()
                .To<DisableInputCommand>()
                .To<WaitBeforeLevelStartCommand>()
                .To<StartGame2CharacterCommand>();

            Container.BindSignalToCommand<Game2Started>()
                .To<EnableInputCommand>()
                .To<SwitchToGame2GameCamera>();

            Container.BindSignalToCommand<Game2Completing>()
                .To<SwitchToGame2EndCamera>()
                .To<StopGame2CharacterCommand>()
                .To<WaitAfterLevelCompletingCommand>()
                .To<SignalWrapper<Game2Completed>>();

            Container.BindSignalToCommand<Game2Completed>();

            Container.BindSignalToCommand<Game2Ending>();

            Container.BindSignalToCommand<Game2Ended>();
            
            Container.BindSignalToCommand<Game2Failing>()
                .To<StopGame2CharacterCommand>();
            
            Container.BindSignalToCommand<Game2Failed>();
            
            Container.BindSignalToCommand<Game2Restarting>()
                .To<WaitFrameCommand>()
                .To<StopGame2CharacterCommand>()
                .To<WaitFrameCommand>()
                .To<SignalWrapper<SwitchingToHome>>();
        }
    }
}
