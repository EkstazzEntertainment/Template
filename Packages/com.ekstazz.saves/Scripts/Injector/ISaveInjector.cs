namespace Ekstazz.Saves
{
    using System;
    using System.Collections.Generic;


    public interface ISaveInjector
    {
        void RegisterSave<T>() where T : class, ISaveComponent;
        void AutoRegisterSaves();
        bool TryInjectSaves(List<ISaveComponent> saves, ISaveable persistent, DateTime lastSaveTime);
        ISaveComponent GetSave(ISaveable persistent);
    }
}