namespace Ekstazz.Saves
{
    using System.Collections.Generic;

    internal interface ISaveContext
    {
        bool IsBlocked { get; }
        
        void ApplyBehaviour(SaveBehaviour saveBehaviour, SaveBlockingContext context);

        List<SaveBlockingContext> SaveBlockers { get; }
    }
    
    internal enum SaveBlockingContext
    {
        Initial,
        Tutorial,
        Reloading,
        Debug
    }
    
    internal enum SaveBehaviour
    {
        SaveDisabled,
        SaveEnabled,
    }
}