namespace Ekstazz.Configs.Cache
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json.Linq;
    using UnityEngine;
    using Zenject;

    internal interface ICacheSaver
    {
        void Save();
        
        Task Load();
    }
    
    internal class CacheSaver : ICacheSaver
    {
        [Inject]
        internal IConfigCache ConfigCache { get; set; }
        
        [Inject]
        internal CacheIoWorker Worker { get; set; }
        
        public void Save()
        {
            var save = ConfigCache.Deserialize();
            var json = JObject.FromObject(save);
            var bytes = Encoding.Unicode.GetBytes(json.ToString());
            Worker.Write(bytes);
            WriteSavedAsJson(json.ToString());
        }
        
        public async Task Load()
        {
            var bytes = await Worker.Read();
            if (bytes == null)
            {
                ConfigCache.Serialize(new CacheSave());
                return;
            }
            var jsonString = Encoding.Unicode.GetString(bytes);
            var json = JObject.Parse(jsonString);
            var configs = json.ToObject<CacheSave>();
            ConfigCache.Serialize(configs);
            WriteLoadedAsJson(json.ToString());
        }
        
        [Conditional("DEBUG")]
        private void WriteLoadedAsJson(string json)
        {
            File.WriteAllText($"{Application.persistentDataPath}/cache.loaded.json", json);
        }
        
        [Conditional("DEBUG")]
        private void WriteSavedAsJson(string json)
        {
            File.WriteAllText($"{Application.persistentDataPath}/cache.saved.json", json);
        }
    }
    
    [Serializable]
    public class CacheSave
    {
        public Dictionary<string, Config> configs = new Dictionary<string, Config>();
    }
}