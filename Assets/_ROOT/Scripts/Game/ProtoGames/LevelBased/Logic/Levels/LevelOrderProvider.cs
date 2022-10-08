namespace Ekstazz.LevelBased.Logic
{
    using System.Collections.Generic;
    using System.Linq;
    using Configs;
    using UnityEngine;
    using Zenject;

    
    public interface ILevelOrderProvider
    {
        LevelOrder GetLevelOrderByNumber(int levelNumber);
    }

    public class LevelOrderProvider : ILevelOrderProvider
    {
        [Inject] private LevelBasedConfigs levelBasedConfigs;

        private bool CyclingEnabled => levelBasedConfigs.LevelsOrder.CyclingEnabled;
        private int CyclingStartNumber => levelBasedConfigs.LevelsOrder.CyclingStartNumber;
        private bool RandomEnabled => levelBasedConfigs.LevelsOrder.RandomEnabled;
        private int RandomStartNumber => levelBasedConfigs.LevelsOrder.RandomStartNumber;
        private List<string> TutorialLevelsOrder => levelBasedConfigs.LevelsOrder.TutorialLevelsOrder;
        private List<string> MainLevelsOrder => levelBasedConfigs.LevelsOrder.MainLevelsOrder;

        private readonly Dictionary<int, LevelOrder> cache = new();

        
        public LevelOrder GetLevelOrderByNumber(int levelNumber)
        {
            if (cache.TryGetValue(levelNumber, out var levelOrder))
            {
                return levelOrder;
            }

            var index = levelNumber - 1;
            if (index < TutorialLevelsOrder.Count)
            {
                levelOrder = new LevelOrder(TutorialLevelsOrder[index], true);
                return CacheOrder(levelOrder, levelNumber);
            }

            index -= TutorialLevelsOrder.Count;
            if (index < MainLevelsOrder.Count)
            {
                levelOrder = new LevelOrder(MainLevelsOrder[index], false);
            }
            else if (!CyclingEnabled)
            {
                levelOrder = new LevelOrder(MainLevelsOrder.Last(), false);
            }
            else if (RandomEnabled)
            {
                var randomIndex = Random.Range(RandomStartNumber - 1, MainLevelsOrder.Count);
                levelOrder = new LevelOrder(MainLevelsOrder[randomIndex], false);
            }
            else
            {
                var cyclingStartIndex = CyclingStartNumber - 1;
                var cycle = (index - MainLevelsOrder.Count) % (MainLevelsOrder.Count - cyclingStartIndex);
                levelOrder = new LevelOrder(MainLevelsOrder[cyclingStartIndex + cycle], false);
            }
            return CacheOrder(levelOrder, levelNumber);
        }

        private LevelOrder CacheOrder(LevelOrder order, int levelNumber)
        {
            cache.Add(levelNumber, order);
            return order;
        }
    }
}
