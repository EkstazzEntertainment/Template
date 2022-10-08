namespace Ekstazz.Saves
{
    using System.Collections.Generic;
    
    public interface ISaveablesRegistry
    {
        IReadOnlyList<ISaveable> Saveables { get; }

        void Register(ISaveable saveable);
    }

    internal class SaveablesRegistry : ISaveablesRegistry
    {
        public IReadOnlyList<ISaveable> Saveables => saveables;
        
        private readonly List<ISaveable> saveables = new List<ISaveable>();
        
        public void Register(ISaveable saveable)
        {
            saveables.Add(saveable);
        }
    }
}