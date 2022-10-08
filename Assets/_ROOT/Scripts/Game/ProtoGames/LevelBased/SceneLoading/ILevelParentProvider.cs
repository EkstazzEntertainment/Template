namespace Ekstazz.LevelBased.SceneLoading
{
    public interface ILevelParentProvider
    {
        ILevelParent LevelParent { get; }
    }

    public interface ILevelParentContainer
    {
        public void UpdateParentTo(ILevelParent newParent);
    }
    
    public class LevelParentProvider : ILevelParentProvider, ILevelParentContainer
    {
        public ILevelParent LevelParent { get; private set; }

        public void UpdateParentTo(ILevelParent newParent)
        {
            LevelParent = newParent;
        }
    }
}