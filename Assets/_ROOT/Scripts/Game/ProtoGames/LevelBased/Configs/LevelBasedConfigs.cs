namespace Ekstazz.LevelBased.Configs
{
    using Ekstazz.Configs;
    using Logic;
    using Ekstazz.LevelBased.Logic;
    using UnityEngine;
    using Zenject;

    
    [AutoConfigurable]
    public class LevelBasedConfigs : IPostProcessable
    {
        [Inject] private ILevelsValidator levelsValidator;

        [ConfigJsonProperty(Key = "levels_order")]
        public LevelsOrder LevelsOrder { get; set; }

        [ConfigJsonProperty(Key = "levels_settings")]
        public LevelSettings LevelSettings { get; set; }
        
        [ConfigJsonProperty(Key = "revive_config")]
        public ReviveConfig ReviveConfig { get; set; }

        [ConfigJsonProperty(IsMultiConfig = true, Key = "level_")]
        public LevelConfig[] LevelConfigs { get; set; }

        
        public void PostProcess()
        {
#if UNITY_EDITOR
            Debug.Log(levelsValidator.Validate(LevelConfigs));
#endif
            foreach (var levelConfig in LevelConfigs)
            {
                levelConfig.PostLoadAction();
            }
        }
    }
}