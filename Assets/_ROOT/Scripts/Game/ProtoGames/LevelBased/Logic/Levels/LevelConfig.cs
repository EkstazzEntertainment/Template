namespace Ekstazz.LevelBased.Logic
{
    using System;
    using System.Collections.Generic;

    
    [Serializable]
    public class LevelConfig
    {
        public string LevelId { get; set; }
        public List<LevelGameInfo> LevelGames { get; set; }
        public Queue<LevelGameInfo> LevelGamesQueue { get; set; } 
        public bool IsTutorial { get; set; }

        
        public void PostLoadAction()
        {
            LevelGamesQueue = new Queue<LevelGameInfo>(LevelGames);
        }
    }
    
    [Serializable]
    public class LevelGameInfo
    {
        public string sceneId;
        public string gameID;
    }
}