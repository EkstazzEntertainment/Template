namespace Ekstazz.ProtoGames.Game1
{
    using Cameras;
    using Flow;
    using Flow.Signals;
    using ProtoGames.Cameras;
    using ProtoGames.Flow;
    using Zenject;

    public class Game1LevelGame : LevelGame
    {
        [Inject] private LazyInject<Game1CamerasController> gameCamerasController;

        public Game1LevelGame(DiContainer container) : base(container)
        {
        }

        public override IGameCamerasController GameCamerasController => gameCamerasController.Value;
        public override string Id => "Game1";
        public override string AreasDirectory => "Game1";
        public override string ScenesPath { get; protected set; } = "Assets/_ROOT/Scenes/Areas/Game1/{0}.unity";

        
        public override void FireLevelReadyToStart()
        {
            SignalBus.AbstractFire<Game1ReadyToStart>();
        }

        public override void FireLevelStarted()
        {
            SignalBus.AbstractFire<Game1Started>();
        }
        
        public override void FireLevelEnding()
        {
            SignalBus.AbstractFire<Game1Ending>();
        }
        
        public override void FireLevelEnded()
        {
            SignalBus.AbstractFire<Game1Ended>();
        }
        
        public override void FireLevelCompleting()
        {
            SignalBus.AbstractFire<Game1Completing>();
        }
        
        public override void FireLevelCompleted()
        {
            SignalBus.AbstractFire<Game1Completed>();
        }

        public override void FireLevelFailed()
        {
            SignalBus.AbstractFire<Game1Failed>();
        }
        
        public override void FireLevelFailing()
        {
            SignalBus.AbstractFire<Game1Failing>();
        }
        
        public override void FireLevelRestarting()
        {
            SignalBus.AbstractFire<Game1Restarting>();
        }
        
        public override void FireModelChanged()
        {
            SignalBus.AbstractFire<ModelChangedInGame1>();
        }
    }
}
