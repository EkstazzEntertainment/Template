namespace Ekstazz.ProtoGames.Flow
{
    using ProtoGames;
    using Zenject.Extensions.Commands;

    public class FireLevelReadyToStartCommand : LockableCommand
    {
        private LevelGameProvider levelGameProvider;
        
        public FireLevelReadyToStartCommand(LevelGameProvider levelGameProvider)
        {
            this.levelGameProvider = levelGameProvider;
        }
        
        public override void Execute()
        {
            levelGameProvider.CurrentLevelGame.FireLevelReadyToStart();
        }
    }
}