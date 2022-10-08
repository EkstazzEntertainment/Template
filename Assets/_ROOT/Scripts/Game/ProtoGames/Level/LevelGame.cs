namespace Ekstazz.ProtoGames
{
    using Cameras;
    using Ekstazz.LevelBased.PrefabLevels;
    using Ekstazz.Ui;
    using Level;
    using Zenject;
    
    
    public abstract class LevelGame
    {
        public virtual string ScenesPath { get; protected set; } = "Assets/_ROOT/Prefabs/Locations/Scenes/{0}.unity";

        [Inject] public UiBuilder UiBuilder { get; set; }
        [Inject] public PrefabLevelBuilder LevelBuilder { get; set; }
        [Inject] public SignalBus SignalBus { get; set; }
        [Inject] public PrefabLevelFactory PrefabLevelFactory { get; set; }
        [InjectOptional] public ILevelViewFinder LevelViewFinder { get; set; }
        [InjectOptional] public LazyInject<Factories.CharacterFactory> CharacterFactory { get; set; }

        public DiContainer Container { get; private set; }
        public virtual IGameCamerasController GameCamerasController { get; set; }

        
        protected LevelGame(DiContainer container)
        {
            Container = container;
        }
        
        public abstract string Id { get; }
        public abstract string AreasDirectory { get; }
        public abstract void FireLevelReadyToStart();
        public abstract void FireLevelStarted();
        public abstract void FireLevelEnding();
        public abstract void FireLevelEnded();
        public abstract void FireLevelCompleting();
        public abstract void FireLevelCompleted();
        public abstract void FireLevelFailed();
        public abstract void FireLevelFailing();
        public abstract void FireLevelRestarting();
        
        public abstract void FireModelChanged();
    }
}
