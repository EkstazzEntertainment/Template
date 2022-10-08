namespace Ekstazz.Saves
{
    using System;

    public interface ISaveable
    {
        /// <summary>
        /// Called on first load to create default save
        /// </summary>
        void PrepareInitial();

        /// <summary>
        /// Called after successful loading
        /// </summary>
        void OnPostLoad(TimeSpan timeSinceSave, DateTime utcNow);
    }
    
    public interface ISaveable<T> : ISaveable where T : ISaveComponent
    {
        T Serialize();

        void Deserialize(T save, DateTime lastSaveTime);
    }
}