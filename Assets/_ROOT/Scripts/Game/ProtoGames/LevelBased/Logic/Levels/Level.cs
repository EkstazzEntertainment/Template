namespace Ekstazz.LevelBased.Logic
{
    using Ekstazz.LevelBased.Logic;

    public class Level
    {
        public int LevelNumber { get; }
        public LevelConfig Config { get; }
        public Area AreaPrefab { get; }
        
        
        public Level(int levelNumber, LevelConfig configuration, Area areaPrefab)
        {
            LevelNumber = levelNumber;
            Config = configuration;
            AreaPrefab = areaPrefab;
        }
    }
}