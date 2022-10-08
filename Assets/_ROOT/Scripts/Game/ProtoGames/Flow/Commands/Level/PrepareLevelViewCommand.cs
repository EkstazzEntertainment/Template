namespace Ekstazz.ProtoGames.Flow
{
    using Zenject;
    using Zenject.Extensions.Commands;
    using ProtoGames;

    public class PrepareLevelViewCommand : LockableCommand
    {
        [Inject] private LevelGameProvider levelGameProvider;

        public override async void Execute()
        {
            var levelViewProvider = levelGameProvider.CurrentLevelGame.LevelViewFinder;
            levelViewProvider?.FindLevelView();
        }
    }
}