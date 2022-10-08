namespace Ekstazz.LevelBased
{
    using Core.Modules;
    using Input.HotKey;
    using Logic;
    using Logic.State;
    using PrefabLevels;
    using Views.Windows;
    using Flow;
    using Flow.Signals;
    using Game.Flow;
    using ProtoGames.Flow;
    using Zenject;
    using Zenject.Extensions.Commands;


    [AutoInstalledModule]
    public class LevelBasedInstaller : ModuleInstaller
    {
        public override IModuleInitializer ModuleInitializer => new Initializer();
        public override string Name => "Game.LevelBased";
        

        public override void InstallBindings()
        {
            Container.BindInterfacesTo<LevelProvider>().AsSingle();
            Container.Bind<ILevelConfigProvider>().To<LevelConfigProvider>().AsSingle();
            Container.Bind<ILevelsValidator>().To<PrefabLevelsValidator>().AsSingle();

            Container.BindInterfacesTo<LevelStateTracker>().AsSingle();
            Container.BindInterfacesTo<TimeCounterController>().AsSingle();
            Container.Bind<ITimeCounter>().To<TimeCounter>().AsTransient();

            Container.DeclareSignal<SwitchingToHome>();
            Container.DeclareSignal<PrepareLevel>();
            Container.DeclareSignal<IProcessFail>();
            
            Container.DeclareSignal<SkippingHome>();
            Container.DeclareSignal<SwitchingToGame>();
            Container.DeclareSignal<LoadLevel>();

            Container.DeclareSignal<ILevelReadyToStart>();
            Container.DeclareSignal<ILevelStarted>();
            Container.DeclareSignal<ILevelCompleting>();
            Container.DeclareSignal<ILevelCompleted>();
            Container.DeclareSignal<ILevelFailing>();
            Container.DeclareSignal<ILevelFailed>();
            Container.DeclareSignal<ILevelEnding>();
            Container.DeclareSignal<ILevelEnded>();
            Container.DeclareSignal<ILevelRestarting>();
            Container.DeclareSignal<ILevelRestarting>();
            Container.DeclareSignal<AllLevelGamesCompleted>();

            Container.BindSignalToCommand<SwitchingToHome>()
                .To<DisableInputCommand>()
                .To<WaitFrameCommand>()
                .To<WindowCreationWrapper<HomeWindow>>()
                .To<EnableInputCommand>();

            Container.BindSignalToCommand<LoadLevel>()
                .To<PreloadLevelCommand>()
                .To<PrepareCurrentLevelCommand>()
                .To<UnloadLastGameSceneCommand>();

            Container.BindSignalToCommand<PrepareLevel>()
                .To<WaitFrameCommand>()
                .To<LoadLevelCommand>()
                .To<SignalWrapper<SubContainersInjected>>()
                .To<PrepareLevelViewCommand>()
                .To<FireLevelReadyToStartCommand>();
            
            Container.BindSignalToCommand<ILevelReadyToStart>();

            Container.BindSignalToCommand<SkippingHome>()
                .To<SignalWrapper<SwitchingToGame>>();

            Container.BindSignalToCommand<SwitchingToGame>()
                .To<WaitFrameCommand>();

            Container.BindSignalToCommand<ILevelStarted>()
                .To<WindowCreationWrapper<GameWindow>>()
                .To<EnableInputCommand>();

            Container.BindSignalToCommand<ILevelCompleting>()
                .To<MoveEarnedCurrencyToWalletCommand>()
                .To<WindowDestructionWrapper<GameWindow>>()
                .To<FireLevelEndingCommand>();

            Container.BindSignalToCommand<ILevelCompleted>()
                .To<FireLevelEndedCommand>()
                .To<TryMoveToNextMiniGameOrLevelCommand>()
                .To<SignalWrapper<LoadLevel>>()
                .To<WindowCreationWrapper<HomeWindow>>();

            Container.BindSignalToCommand<AllLevelGamesCompleted>()
                .To<SignalWrapper<SwitchingToHome>>();

            Container.BindSignalToCommand<IProcessFail>()
                .To<TryReviveCommand>()
                .To<FireLevelFailedCommand>();

            Container.BindSignalToCommand<ILevelFailing>()
                .To<ClearAccumulatedTemporaryCurrency>()
                .To<WaitAfterLevelFailingCommand>()
                .To<WindowDestructionWrapper<GameWindow>>()
                .To<WindowCreationWrapper<LevelFailedWindow>>()
                .To<FireLevelEndingCommand>();

            Container.BindSignalToCommand<ILevelFailed>()
                .To<ClearAccumulatedTemporaryCurrency>()
                .To<ReloadCurrentLevelGamesQueueCommand>()
                .To<SignalWrapper<LoadLevel>>()
                .To<WindowDestructionWrapper<GameWindow>>()
                .To<FireLevelEndedCommand>()
                .To<WindowCreationWrapper<HomeWindow>>()
                .To<SignalWrapper<SkippingHome>>();

            Container.BindSignalToCommand<ILevelEnding>()
                .To<DisableInputCommand>();

            Container.BindSignalToCommand<ILevelEnded>();

            Container.BindSignalToCommand<ILevelRestarting>()
                .To<WindowDestructionWrapper<GameWindow>>()
                .To<ClearAccumulatedTemporaryCurrency>()
                .To<ReloadCurrentLevelGamesQueueCommand>()
                .To<SignalWrapper<LoadLevel>>()
                .To<WaitFrameCommand>()
                .To<FireLevelEndingCommand>();
        }

        private class Initializer : IModuleInitializer
        {
            [Inject] private IHotKeyDebug hotKeyDebug;

            public void Prepare()
            {
                // hotKeyDebug.RegisterAction(new LevelFailAction());
                // hotKeyDebug.RegisterAction(new LevelCompleteAction());
                // hotKeyDebug.RegisterAction(new LevelRestartAction());
            }
        }
    }
}
