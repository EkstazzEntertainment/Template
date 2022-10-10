namespace Ekstazz.Saves.General.Components
{
    using System;
    using Core;
    using Zenject;

    
    public abstract class SaveableComponent<T> : IInitializable, ISaveable<T> where T : ISaveComponent, new()
    {
        [Inject] private ISaveScheduler SaveScheduler { get; set; }
        [Inject] private ISaveablesRegistry SaveablesRegistry { get; set; } 
        
        public virtual Priority LoadPriority => Priority.Simple;

        
        public virtual void Initialize()
        {
            SaveablesRegistry.Register(this);
        }

        public void PrepareInitial()
        {
            Deserialize(PrepareInitialSave(), DateTime.UtcNow);
        }

        protected virtual T PrepareInitialSave()
        {
            return new T();
        }

        protected void ScheduleSave()
        {
            SaveScheduler.ScheduleSave();
        }

        public virtual void OnPostLoad(TimeSpan timeSinceSave, DateTime utcNow) { }
        public abstract T Serialize();
        public abstract void Deserialize(T save, DateTime lastSaveTime);
    }
}