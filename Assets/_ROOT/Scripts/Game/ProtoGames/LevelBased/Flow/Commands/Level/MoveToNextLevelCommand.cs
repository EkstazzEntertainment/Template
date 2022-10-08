namespace Ekstazz.LevelBased.Flow
{
    using System.Threading.Tasks;
    using Ekstazz.LevelBased.Logic;
    using Zenject;
    using Zenject.Extensions.Commands;

    public class MoveToNextLevelCommand : Command
    {
        [Inject] private ILevelProvider levelProvider;

        
        public override async Task Execute()
        {
            levelProvider.MoveToNextLevel();
        }
    }
}