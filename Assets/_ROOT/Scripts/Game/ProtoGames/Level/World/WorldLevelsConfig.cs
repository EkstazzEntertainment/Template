namespace Ekstazz.ProtoGames.Level.World.Config
{
    using System;
    using System.Collections.Generic;
    using Ekstazz.Configs;

    
    [AutoConfigurable]
    public class WorldLevelsConfig
    {
        [ConfigJsonProperty(Key = "world_levels")]
        public WorldThemeLevelsConfig WorldThemeLevelsConfig { get; set; }
    }

    [Serializable]
    public class WorldThemeLevelsConfig
    {
        public List<GameWorldThemeInfo> GameWorldThemeInfos { get; set; }
    }

    [Serializable]
    public class GameWorldThemeInfo
    {
        public string GameID { get; set; }
        public List<WorldThemeLevel> Configurations { get; set; }
    }
    
    [Serializable]
    public class WorldThemeLevel
    {
        public string WorldId { get; set; }
        public List<string> Levels { get; set; }
    }
}
