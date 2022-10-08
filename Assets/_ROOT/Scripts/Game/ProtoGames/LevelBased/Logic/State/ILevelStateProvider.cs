namespace Ekstazz.LevelBased.Logic.State
{
    using System;
    
    public interface ILevelStateProvider
    {
        /// <summary>
        /// Event for changing state from arg1 value to arg2 value
        /// </summary>
        event Action<LevelState, LevelState> LevelStateChanged;

        LevelState CurrentState { get; }
    }
}
