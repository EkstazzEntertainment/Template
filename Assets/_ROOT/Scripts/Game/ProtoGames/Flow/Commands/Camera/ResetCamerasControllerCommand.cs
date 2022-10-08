namespace Ekstazz.ProtoGames.Flow
{
    using Zenject;
    using Zenject.Extensions.Commands;

    public class ResetCamerasControllerCommand : LockableCommand
    {
        [Inject] private LevelGameProvider levelGameProvider;
        
        public override void Execute()
        {
            levelGameProvider.CurrentLevelGame.GameCamerasController.ResetCameras();
        }
    }
}