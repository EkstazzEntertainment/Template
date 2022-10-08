namespace Ekstazz.Configs
{
    using System;
    using UnityEngine;

    
    internal class BackedUpConfigApplier
    {
        public IConfigProvider Primary;
        public IConfigProvider Secondary;

        
        public virtual void ApplyConfigOf(string key, Action<string> action, bool useMultiConfig)
        {
            try
            {
                ApplyConfigByProvider(key, action, useMultiConfig, Primary);
            }
            catch (Exception e)
            {
                Debug.LogError($"Exception while applying primary config values for key {key}, mc: {useMultiConfig}:\r\n{e}");
                try
                {
                    ApplyConfigByProvider(key, action, useMultiConfig, Secondary);
                }
                catch (Exception e2)
                {
                    Debug.LogError($"Exception while applying secondary (!!!) config values for key {key}, mc: {useMultiConfig}:\r\n{e2}");
                    throw;
                }
            }
        }

        private void ApplyConfigByProvider(string key, Action<string> action, bool useMultiConfig, IConfigProvider configProvider)
        {
            var value = useMultiConfig ? configProvider.GetMultiConfigOf(key) : configProvider.GetConfigOf(key); action(value);
        }
    }
}