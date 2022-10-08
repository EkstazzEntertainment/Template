namespace Ekstazz.Saves
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Because we have two genericly typed lists - IPersistent and IComponentSave,
    /// we need special entity for performing injection concrete T : IComponentSave into IPersistent<T>
    /// </summary>
    /// 
    public interface ISaveInjector
    {
        void RegisterSave<T>() where T : class, ISaveComponent;
        void AutoRegisterSaves();
        bool TryInjectSaves(List<ISaveComponent> saves, ISaveable persistent, DateTime lastSaveTime);
        ISaveComponent GetSave(ISaveable persistent);
    }
}