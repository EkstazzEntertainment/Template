namespace Ekstazz.LevelBased.Flow
{
    using System.Threading.Tasks;
    using Ekstazz.LevelBased.Logic;
    using Ekstazz.LevelBased.SceneLoading;
    using Zenject;
    using Zenject.Extensions.Commands;

    
    public class PreloadLevelCommand : Command
    {
        [Inject] private ILevelParentProvider levelParentProvider;
        [Inject] private ILevelProvider levelProvider;

        
        public override Task Execute()
        {
            var currentGameInfo = levelProvider.CurrentLevelGameInfo;
            levelParentProvider.LevelParent.PreloadLevel(currentGameInfo.sceneId);
            return Task.CompletedTask;
        }
    }
    
    public class TryPreloadLevelCommand : Command
    {
        [Inject] private ILevelParentProvider levelParentProvider;
        [Inject] private ILevelProvider levelProvider;

        
        public override Task Execute()
        {
            if (levelProvider.CurrentLevel.Config.LevelGamesQueue.Count != 0)
            {
                var currentGameInfo = levelProvider.CurrentLevelGameInfo;
                levelParentProvider.LevelParent.PreloadLevel(currentGameInfo.sceneId);
            }
            
            return Task.CompletedTask;
        }
    }
}