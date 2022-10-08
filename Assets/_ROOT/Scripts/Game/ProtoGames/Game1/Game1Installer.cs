namespace Ekstazz.ProtoGames.Game1
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


    public class Game1Installer : Installer<Game1Installer>
    {
        private string settingsPath = "Game1/Settings";


        public override void InstallBindings()
        {
            Container.BindFactory<Area, Transform, Area, PrefabLevelFactory>()
                .FromFactory<PrefabLevelFactory>();

            Container.Bind<LevelGame>().To<Game1LevelGame>().AsSingle();
            Container.Bind(typeof(ILevelViewProvider<Game1LevelView>), typeof(ILevelViewFinder))
                .To<LevelViewProvider<Game1LevelView>>().AsSingle();

            Container.Bind<PrefabLevelBuilder>().To<Game1PrefabBuilder>().AsSingle();
            Container.Bind<UiBuilder>().To<Game1UiBuilder>().AsSingle();
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
            Container.DeclareSignal<Game1ReadyToStart>();
            Container.DeclareSignal<Game1Started>();
            Container.DeclareSignal<Game1Completing>();
            Container.DeclareSignal<Game1Completed>();
            Container.DeclareSignal<Game1Failing>();
            Container.DeclareSignal<Game1Failed>();
            Container.DeclareSignal<Game1Ending>();
            Container.DeclareSignal<Game1Ended>();
            Container.DeclareSignal<Game1Restarting>();

            Container.DeclareSignal<ModelChangedInGame1>();
        }

        private void InstallResources()
        {
            Container.Bind<Game1SettingsForTestingPurposes>().FromResourceSettings(settingsPath);
        }

        private void InstallFactories()
        {
            Container.BindFactory<Object, CharacterView, Factories.CharacterFactory>()
                .FromFactory<PrefabFactory<CharacterView>>();
        }

        private void InstallProviders()
        {
            Container.BindInterfacesAndSelfTo<Game1Input>().AsSingle();
            Container.BindInterfacesTo<Game1WorldThemeProvider>().AsSingle();
        }

        private void InstallControllersAndHandlers()
        {
            Container.BindInterfacesAndSelfTo<LevelStyleAdjuster<Game1ReadyToStart>>().AsSingle();
            Container.BindInterfacesAndSelfTo<Game1CharacterMovement>().AsSingle();
            Container.BindInterfacesAndSelfTo<Game1CharacterRotation>().AsSingle();
            Container.BindInterfacesAndSelfTo<Game1CharacterAnimator>().AsSingle();
            Container.BindInterfacesAndSelfTo<Game1LevelCameraAdjuster>().AsSingle();
        }

        private void InstallLevelGameFlow()
        {
            Container.BindSignalToCommand<Game1ReadyToStart>()
                .To<WaitFrameCommand>()
                .To<SpawnGame1CharacterCommand>()
                .To<SwitchToGame1HomeCamera>()
                .To<DisableInputCommand>()
                .To<WaitBeforeLevelStartCommand>()
                .To<StartGame1CharacterCommand>();

            Container.BindSignalToCommand<Game1Started>()
                .To<EnableInputCommand>()
                .To<SwitchToGame1GameCamera>();

            Container.BindSignalToCommand<Game1Completing>()
                .To<SwitchToGame1EndCamera>()
                .To<StopGame1CharacterCommand>()
                .To<WaitAfterLevelCompletingCommand>()
                .To<SignalWrapper<Game1Completed>>();

            Container.BindSignalToCommand<Game1Completed>();

            Container.BindSignalToCommand<Game1Ending>();

            Container.BindSignalToCommand<Game1Ended>();
            
            Container.BindSignalToCommand<Game1Failing>()
                .To<StopGame1CharacterCommand>();
            
            Container.BindSignalToCommand<Game1Failed>();
            
            Container.BindSignalToCommand<Game1Restarting>()
                .To<WaitFrameCommand>()
                .To<StopGame1CharacterCommand>()
                .To<WaitFrameCommand>()
                .To<SignalWrapper<SwitchingToHome>>();
        }
    }
}
