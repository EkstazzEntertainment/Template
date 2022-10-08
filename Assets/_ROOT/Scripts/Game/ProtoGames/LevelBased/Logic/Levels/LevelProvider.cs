namespace Ekstazz.LevelBased.Logic
{
    using System;
    using Configs;
    using Ekstazz.Saves.General.Components;
    using Saves;
    using Ekstazz.LevelBased.Flow.Signals;
    using Ekstazz.ProtoGames;
    using Zenject;

    
    public interface ILevelProvider
    {
        int CurrentLevelNumber { get; }
        Level CurrentLevel { get; }
        LevelGameInfo CurrentLevelGameInfo { get; }
        void MoveToNextLevel();
        void MoveToNextLevel(int levelNumber);
        void TryMoveToNextMiniGameOrLevel();
        void ReloadLevelQueue();
        Level GetLevelByNumber(int levelNumber);
        int GetOverallLevelCount();
    }
    
    public class LevelProvider : SaveableComponent<LevelSave>, ILevelProvider
    {
        [Inject] private SignalBus signalBus;
        [Inject] private LevelGameProvider levelGameProvider;
        [Inject] private LevelBasedConfigs configs;
        [Inject] private ILevelConfigProvider levelConfigProvider;

        public int CurrentLevelNumber { get; private set; }
        public Level CurrentLevel { get; private set; }
        public LevelGameInfo CurrentLevelGameInfo { get; private set; }

        
        public void MoveToNextLevel()
        {
            MoveToNextLevel(CurrentLevelNumber + 1);
        }

        public void MoveToNextLevel(int levelNumber)
        {
            var level = GetLevelByNumber(levelNumber);
            CurrentLevel = level;
            CurrentLevelNumber = levelNumber;

            ScheduleSave();
        }

        public void ReloadLevelQueue()
        {
            MoveToNextLevel(CurrentLevelNumber);
        }

        private void ChangeCurrentGame(LevelConfig currLvlCfg)
        {
            CurrentLevelGameInfo = currLvlCfg.LevelGamesQueue.Dequeue();
            levelGameProvider.ChangeCurrentGame(CurrentLevelGameInfo.gameID);
        }

        public void TryMoveToNextMiniGameOrLevel()
        {
            if (CurrentLevel.Config.LevelGamesQueue.Count == 0)
            {
                MoveToNextLevel();
                signalBus.Fire<AllLevelGamesCompleted>();
            }
            else
            {
                ChangeCurrentGame(CurrentLevel.Config);
            }
        }

        public int GetOverallLevelCount()
        {
            return configs.LevelsOrder.MainLevelsOrder.Count;
        }

        public Level GetLevelByNumber(int levelNumber)
        {
            var order = configs.LevelsOrder.GetLevelOrder(levelNumber);
            var config = levelConfigProvider.GetConfigFor(order.LevelId);
            ChangeCurrentGame(config);

            return levelGameProvider.CurrentLevelGame.LevelBuilder.BuildLevel(levelNumber, config);
        }

        protected override LevelSave PrepareInitialSave()
        {
            return new LevelSave
            {
                Level = 1
            };
        }

        public override LevelSave Serialize()
        {
            return new LevelSave
            {
                Level = CurrentLevelNumber
            };
        }

        public override void Deserialize(LevelSave save, DateTime lastSaveTime)
        {
            CurrentLevelNumber = save.Level;
            CurrentLevel = GetLevelByNumber(save.Level);
        }
    }
}