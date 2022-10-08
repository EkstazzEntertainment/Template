namespace Ekstazz.ProtoGames.Game1.Flow
{
    using Game1;
    using ProtoGames.Level;
    using Zenject;
    using Zenject.Extensions.Commands;

    
    public class StartGame1CharacterCommand : LockableCommand
    {
        [Inject] private ILevelViewProvider<Game1LevelView> levelViewProvider;

        public override void Execute()
        {
            levelViewProvider.LevelView.PlayerView.StartCharacter();
        }
    }
}
