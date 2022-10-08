namespace Ekstazz.LevelBased.PrefabLevels
{
    using System.Collections.Generic;
    using Logic;
    using Ekstazz.LevelBased.Logic;

    public class PrefabLevelBuilder : ILevelBuilder
    {
        private readonly Dictionary<string, Area> cache = new Dictionary<string, Area>();

        public Level BuildLevel(int levelNumber, LevelConfig config)
        {
            return new Level(levelNumber, config, null);
        }
    }
}