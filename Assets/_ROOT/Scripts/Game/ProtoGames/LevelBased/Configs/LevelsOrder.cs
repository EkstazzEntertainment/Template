namespace Ekstazz.LevelBased.Configs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Random = UnityEngine.Random;

    
    [Serializable]
    public class LevelsOrder
    {
        public bool CyclingEnabled { get; set; }
        public int CyclingStartNumber { get; set; } = 1;
        public bool RandomEnabled { get; set; }
        public int RandomStartNumber { get; set; } = 1;
        public List<string> TutorialLevelsOrder { get; set; }
        public List<string> MainLevelsOrder { get; set; }
        
        private readonly Dictionary<int, OrderedLevel> cache = new Dictionary<int, OrderedLevel>();
        
        
        public OrderedLevel GetLevelOrder(int levelNumber)
        {
            if (cache.TryGetValue(levelNumber, out var order))
            {
                return order;
            }
            
            var resultIndex = levelNumber - 1;
            if (resultIndex < TutorialLevelsOrder.Count)
            {
                order = new OrderedLevel(TutorialLevelsOrder[resultIndex], true);
                return CacheOrder(order, levelNumber);
            }
            
            resultIndex -= TutorialLevelsOrder.Count;
            if (resultIndex < MainLevelsOrder.Count)
            {
                order = new OrderedLevel(MainLevelsOrder[resultIndex], false);
            }
            else if (!CyclingEnabled)
            {
                order = new OrderedLevel(MainLevelsOrder.Last(), false);
            }
            else if (RandomEnabled)
            {
                var randomIndex = Random.Range(RandomStartNumber - 1, MainLevelsOrder.Count);
                order = new OrderedLevel(MainLevelsOrder[randomIndex], false);
            }
            else
            {
                var cyclingStartIndex = CyclingStartNumber - 1;
                var cycle = (resultIndex - MainLevelsOrder.Count) % (MainLevelsOrder.Count - cyclingStartIndex);
                order = new OrderedLevel(MainLevelsOrder[cyclingStartIndex + cycle], false);
            }
            
            return CacheOrder(order, levelNumber);
        }

        private OrderedLevel CacheOrder(OrderedLevel order, int levelNumber)
        {
            cache.Add(levelNumber, order);
            return order;
        }
    }

    public class OrderedLevel
    {
        public string LevelId { get; }
        public bool IsTutorial { get; }
        
        
        public OrderedLevel(string levelId, bool isTutorial)
        {
            LevelId = levelId;
            IsTutorial = isTutorial;
        }
    }
}