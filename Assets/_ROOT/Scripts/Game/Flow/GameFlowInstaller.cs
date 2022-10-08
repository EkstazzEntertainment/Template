namespace Ekstazz.Game.Flow
{
    using Ekstazz.Configs.Flow;
    using Core;
    using Core.Modules;
    using Ekstazz.Saves.Flow;
    using Zenject;
    using Zenject.Extensions.Commands;


    [AutoInstalledModule]
    public class GameFlowInstaller : ModuleInstaller
    {
        public override string Name => "Game.Flow";
        public override IModuleInitializer ModuleInitializer => new GameFlowInitializer();

        
        public override void InstallBindings()
        {
            Container.Bind<ISplash>().To<Splash>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameStartedAwaiter>().AsSingle();

            Container.DeclareSignal<GameSceneLoaded>();

            Container.BindSignalToCommand<StartApp>()
                .To<GlobalWindowCreationWrapper<LoadingWindow>>()
                .To<InitConfigsFetchingCommand>()
                .To<StartProcessingSavesCommand>()
                .To<StartModulesCommand>()
                .To<ApplyFetchedConfigsCommand>()
                .To<WaitAndApplySaveCommand>()
                .To<TryUnblockSavesCommand>()
                .To<SwitchToGameCommand>()
                .To<SignalWrapper<GameSceneLoaded>>()
                .To<AwaitLoadingWindowCommand>()
                .To<WaitAfterLoadingFinishedCommand>()
                .To<GlobalWindowDestructionWrapper<LoadingWindow>>();
        }   

        private class GameFlowInitializer : IModuleInitializer
        {
            public void Prepare()
            {
            }
        }
    }
}