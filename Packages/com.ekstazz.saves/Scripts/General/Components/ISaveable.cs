namespace Ekstazz.Saves
{
    using System;

    
    public interface ISaveable
    {
        void PrepareInitial();
        void OnPostLoad(TimeSpan timeSinceSave, DateTime utcNow);
    }
    
    public interface ISaveable<T> : ISaveable where T : ISaveComponent
    {
        T Serialize();
        void Deserialize(T save, DateTime lastSaveTime);
    }
}