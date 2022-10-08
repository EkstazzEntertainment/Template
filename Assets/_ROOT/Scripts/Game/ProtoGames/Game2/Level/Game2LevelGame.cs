namespace Ekstazz.ProtoGames.Game2
{
    using Cameras;
    using Flow;
    using Flow.Signals;
    using ProtoGames.Cameras;
    using ProtoGames.Flow;
    using Zenject;

    public class Game2LevelGame : LevelGame
    {
        [Inject] private LazyInject<Game2CamerasController> gameCamerasController;

        public Game2LevelGame(DiContainer container) : base(container)
        {
        }

        public override IGameCamerasController GameCamerasController => gameCamerasController.Value;
        public override string Id => "Game2";
        public override string AreasDirectory => "Game2";
        public override string ScenesPath { get; protected set; } = "Assets/_ROOT/Scenes/Areas/Game2/{0}.unity";

        
        public override void FireLevelReadyToStart()
        {
            SignalBus.AbstractFire<Game2ReadyToStart>();
        }

        public override void FireLevelStarted()
        {
            SignalBus.AbstractFire<Game2Started>();
        }
        
        public override void FireLevelEnding()
        {
            SignalBus.AbstractFire<Game2Ending>();
        }
        
        public override void FireLevelEnded()
        {
            SignalBus.AbstractFire<Game2Ended>();
        }
        
        public override void FireLevelCompleting()
        {
            SignalBus.AbstractFire<Game2Completing>();
        }
        
        public override void FireLevelCompleted()
        {
            SignalBus.AbstractFire<Game2Completed>();
        }

        public override void FireLevelFailed()
        {
            SignalBus.AbstractFire<Game2Failed>();
        }
        
        public override void FireLevelFailing()
        {
            SignalBus.AbstractFire<Game2Failing>();
        }
        
        public override void FireLevelRestarting()
        {
            SignalBus.AbstractFire<Game2Restarting>();
        }
        
        public override void FireModelChanged()
        {
            SignalBus.AbstractFire<ModelChangedInGame2>();
        }
    }
}
