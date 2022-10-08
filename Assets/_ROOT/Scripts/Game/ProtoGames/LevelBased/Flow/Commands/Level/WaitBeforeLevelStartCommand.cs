namespace Ekstazz.LevelBased.Flow
{
    using Ekstazz.LevelBased.Configs;
    using ProtoGames.Flow;
    using Zenject;

    
    public class WaitBeforeLevelStartCommand : AwaitTimeCommand
    {
        [Inject] private LevelBasedConfigs config;

        protected override float AwaitTime => config.LevelSettings.LevelStartDelay;
    }
}
