namespace Ekstazz.Saves
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using Zenject;

    
    internal interface ISaver
    {   
        void LoadFromSave(SaveData saveData);
        SaveData CreateSave();
    }
    

    internal class Saver : ISaver
    {
        [Inject] internal ISaveablesRegistry SaveablesRegistry { get; set; }
        [Inject] internal ISaveInjector SaveInjector { get; set; }

        
        public void LoadFromSave(SaveData saveData)
        {
            var saveables = SaveablesRegistry.Saveables;

            if (saveData.IsInitial)
            {
                InitialStartSequence(saveables);
            }
            else
            {
                UsualSequence(saveData, saveables, saveData.Result.Header.SaveTimestamp);
            }
        }

        private void UsualSequence(SaveData saveData, IReadOnlyList<ISaveable> persistents, DateTime lastSaveTime)
        {
            foreach (var persistent in persistents)
            {
                if (!SaveInjector.TryInjectSaves(saveData.Result.Components, persistent, lastSaveTime))
                {
                    throw new Exception($"Could not insert into {persistent}");
                }
            }

            var timeSinceSave = DateTime.UtcNow - saveData.Result.Header.SaveTimestamp;
            foreach (var persistent in persistents)
            {
                persistent.OnPostLoad(timeSinceSave, DateTime.UtcNow);
            }
        }

        private static void InitialStartSequence(IReadOnlyList<ISaveable> saveables)
        {
            foreach (var persistent in saveables)
            {
                persistent.PrepareInitial();
            }
        }

        public SaveData CreateSave()
        {
            var header = new SerializationHeader(DateTime.UtcNow);
            var components = SaveablesRegistry.Saveables.Select(persistent => SaveInjector.GetSave(persistent)).ToList();

            return new LocalSaveData(new SaveModel(header, components));
        }
    }
}