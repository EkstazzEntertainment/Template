namespace Ekstazz.LevelBased.Logic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Configs;
    using ExtensionsAndHelpers;
    using Zenject;

    public interface ILevelConfigProvider
    {
        LevelConfig GetConfigFor(string levelId);
        List<LevelConfig> GetAllConfigs();
    }
    
    
    public class LevelConfigProvider : ILevelConfigProvider
    {
        [Inject] private LevelBasedConfigs levelBasedConfigs;
        
        
        public LevelConfig GetConfigFor(string levelId)
        {
            var levelConfig = levelBasedConfigs.LevelConfigs.FirstOrDefault(config => config.LevelId == levelId);
            if (levelConfig == null)
            {
                throw new ArgumentException($"Cannot find config for {levelId}");
            }

            return levelConfig.DeepCopy<LevelConfig>();
        }

        public List<LevelConfig> GetAllConfigs()
        {
            return levelBasedConfigs.LevelConfigs.ToList();
        }
    }
}