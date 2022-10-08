namespace Ekstazz.Configs.Resources
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using UnityEngine;

    internal class ResourcesConfigWrapper : IConfigServiceWrapper
    {
        private Dictionary<string, string> values;

        public virtual Task FetchAsync()
        {
            var allAssets = Resources.LoadAll<TextAsset>("Configs/");
            values = allAssets.ToDictionary(asset => asset.name, asset => asset.text);
            return Task.FromResult(true);
        }

        public virtual bool ApplyFetched()
        {
            return true;
        }

        public virtual IEnumerable<string> GetKeys(string configNamespace)
        {
            foreach (var key in values.Keys)
            {
                yield return key;
            }
        }

        public virtual IConfigValueWrapper GetValue(string key, string configNamespace)
        {
            var value = values[key];
            return new SimpleValueWrapper(value, s => s.Replace("<NEWLINE>", "\r\n"));
        }

        public void LogExceptionDetails(AggregateException exception)
        {
            Debug.LogException(exception);
        }
    }
    
    public class ResourcesConfigs : IConfigServiceProvider
    {
        public string Name => "Resources Configs";
        public IConfigServiceWrapper CreateWrapper() => new ResourcesConfigWrapper();
        public int Priority => 0;
    }
}