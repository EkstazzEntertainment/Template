namespace Ekstazz.LevelBased.Logic
{
    using Ekstazz.LevelBased.Configs;
    using Ekstazz.LevelBased.Logic;
    using Flow.Signals;
    using Zenject;

    
    public class ReviveController : IInitializable
    {
        [Inject] private ILevelProvider levelProvider;
        [Inject] private SignalBus signalBus;
        [Inject] private LevelBasedConfigs levelBasedConfigs;
        
        private ReviveConfig ReviveConfig => levelBasedConfigs.ReviveConfig;

        private int revivingUsed;
        private int lastRevivingUsedLevel;
        private bool revivingWasUsed;

        
        public void Initialize()
        {
            signalBus.Subscribe<ILevelCompleting>(OnLevelCompleting);
            signalBus.Subscribe<ILevelEnding>(OnLevelEnding);
        }

        public void ApplyReviving()
        {
            revivingWasUsed = true;
            revivingUsed += 1;
        }

        public bool CanRevive()
        {
            var isTutorial = levelProvider.CurrentLevel.Config.IsTutorial;
            var levelsFromLastRevive = levelProvider.CurrentLevelNumber - lastRevivingUsedLevel;
            return !isTutorial &&
                   levelsFromLastRevive >= ReviveConfig.LevelsBetweenProposals &&
                   revivingUsed < ReviveConfig.RevivesPerAttempt;
        }

        private void OnLevelCompleting()
        {
            if (revivingWasUsed)
            {
                revivingWasUsed = false;
                lastRevivingUsedLevel = levelProvider.CurrentLevelNumber - 1;
            }
        }
        
        private void OnLevelEnding()
        {
            revivingUsed = 0;
        }
    }
}