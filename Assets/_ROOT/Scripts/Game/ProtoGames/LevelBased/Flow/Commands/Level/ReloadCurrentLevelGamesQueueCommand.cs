namespace Ekstazz.LevelBased.Flow
{
    using Ekstazz.LevelBased.Logic;
    using Zenject;
    using Zenject.Extensions.Commands;

    public class ReloadCurrentLevelGamesQueueCommand : LockableCommand
    {
        [Inject] private ILevelProvider levelProvider;

        public override void Execute()
        {
            levelProvider.ReloadLevelQueue();
        }
    }
}
