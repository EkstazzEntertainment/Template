namespace Ekstazz.ProtoGames.Flow
{
    using System.Threading.Tasks;
    using ProtoGames;
    using Zenject;
    using Zenject.Extensions.Commands;

    public class FireLevelEndingCommand : Command
    {
        [Inject] private LevelGameProvider levelGameProvider;

        public override async Task Execute()
        {
            levelGameProvider.CurrentLevelGame.FireLevelEnding();
        }
    }
}