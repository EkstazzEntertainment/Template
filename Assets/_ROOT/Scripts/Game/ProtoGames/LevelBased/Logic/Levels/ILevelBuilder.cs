namespace Ekstazz.LevelBased.Logic
{
    public interface ILevelBuilder
    {
        Level BuildLevel(int levelNumber, LevelConfig config);
    }
}