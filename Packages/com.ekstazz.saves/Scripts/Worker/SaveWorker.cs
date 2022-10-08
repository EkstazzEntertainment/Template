namespace Ekstazz.Saves.Worker
{
    using System.Diagnostics;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using Data;
    using Packers;
    using Parsers;
    using UnityEngine;
    using Zenject;

    internal class SaveWorker : ISaveWorker
    {
        [Inject(Id = SaveOrigin.Local)]
        public ISaveIoWorker LocalWorker { get; set; }

        [Inject(Id = SaveOrigin.Remote)]
        public ISaveIoWorker RemoteWorker { get; set; }

        [Inject]
        public ISavePacker SavePacker { get; set; }

        [Inject]
        public ISaveParser SaveParser { get; set; }
        
        [Inject]
        public SavePipeline SavePipeline { get; set; }
        
        private static readonly object LockingObject = new object();
        
        public async void Write(SaveData saveData)
        {
            WriteUnpackedSaveAsJson(saveData);

            var raw = await Task.Run(() => GetRawSave(saveData));

            // Monitor to prevent simultaneous writing to the save save-file
            var lockTaken = false;
            Monitor.Enter(LockingObject, ref lockTaken);
            if (!lockTaken)
            {
                return;
            }
            try
            {
                //we are checking only local saver, because remote saver can take to much time to process
                await LocalWorker.Write(raw);
            }
            finally
            {
                Monitor.Exit(LockingObject);
            }

            await RemoteWorker.Write(raw);    
        }
        
        private byte[] GetRawSave(SaveData saveData)
        {
            var jObject = SaveParser.ToJson(saveData.Result);
            var raw = SavePacker.Pack(jObject);

            return raw;
        }

        [Conditional("UNITY_EDITOR")]
        private void WriteUnpackedSaveAsJson(SaveData result)
        {
            File.WriteAllText($"{Application.persistentDataPath}/savefile.unpacked.json", SavePipeline.ToJson(result.Result));
        }
    }
}