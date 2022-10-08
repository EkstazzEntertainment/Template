namespace Ekstazz.LevelBased.Logic.State
{
    public enum LevelState
    {
        None = 1,
        Preparing = 2,
        Prepared = 4,
        Started = 8,
        Completing = 16,
        Failing = 32,
        Completed = 64,
        Failed = 128,
    }

    public static class LevelStateExtensions
    {
        public static bool IsEnding(this LevelState state)
        {
            return state == LevelState.Completing || state == LevelState.Failing;
        }

        public static bool IsEnded(this LevelState state)
        {
            return state == LevelState.Completed || state == LevelState.Failed;
        }
    }
}