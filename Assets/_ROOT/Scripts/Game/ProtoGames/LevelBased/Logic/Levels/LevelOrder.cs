namespace Ekstazz.LevelBased.Logic
{
    public class LevelOrder
    {
        public string LevelId { get; }
        public bool IsTutorial { get; }

        public LevelOrder(string levelId, bool isTutorial)
        {
            LevelId = levelId;
            IsTutorial = isTutorial;
        }
    }
}
