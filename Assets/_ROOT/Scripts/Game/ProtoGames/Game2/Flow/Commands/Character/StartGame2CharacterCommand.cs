namespace Ekstazz.ProtoGames.Game2.Flow
{
    using Game2;
    using ProtoGames.Level;
    using Zenject;
    using Zenject.Extensions.Commands;

    
    public class StartGame2CharacterCommand : LockableCommand
    {
        [Inject] private ILevelViewProvider<Game2LevelView> levelViewProvider;

        public override void Execute()
        {
            levelViewProvider.LevelView.PlayerView.StartCharacter();
        }
    }
}
