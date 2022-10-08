namespace Ekstazz.Configs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using UnityEngine;

    internal class ConfigFetcher : IConfigFetcher
    {
        private Task fetchingTask;
        private bool wasProvidedWithDefaultValues;
        private readonly Dictionary<string, string> configs = new Dictionary<string, string>();
        private bool wasFetchingTimedOut;
        private bool wasStarted;
        private bool isLoaded;

        public bool IsLoaded
        {
            get => isLoaded;
            private set
            {
                Debug.Log($"Setting Fetcher IsLoaded to {value}");
                isLoaded = value;
            }
        }

        public IConfigServiceWrapper ServiceWrapper { get; set; }

        public void StartFetching()
        {
            ThrowIfWasStarted();
            Task firebaseFetchTask;
            try
            {
                firebaseFetchTask = ServiceWrapper.FetchAsync();
            }
            catch (Exception e)
            {
                firebaseFetchTask = Task.FromException(e);
            }

            fetchingTask = firebaseFetchTask.ContinueWith(OnFetchOver);
            wasStarted = true;
        }

        private void OnFetchOver(Task task)
        {
            Debug.Log($"Fetch over.");
            IsLoaded = !(task.IsCanceled || task.IsFaulted || wasFetchingTimedOut);

            if (IsLoaded)
            {
                PopulateConfigsWithServerValues();
            }
            else
            {
                Debug.LogError(
                    $"Remote config loading error: canceled = {task.IsCanceled}, faulted = {task.IsFaulted}, timedOut = {wasFetchingTimedOut}");
                if (task.Exception != null)
                {
                    ServiceWrapper.LogExceptionDetails(task.Exception);
                }
            }

            if (wasProvidedWithDefaultValues)
            {
                IsLoaded = true;
            }
        }

        private void PopulateConfigsWithServerValues()
        {
            ServiceWrapper.ApplyFetched();
            var keys = ServiceWrapper.GetKeys(string.Empty);
            foreach (var key in keys)
            {
                var configValue = ServiceWrapper.GetValue(key, string.Empty);

                var value = configValue.StringValue;
                configs[Unify(key)] = value;
            }
        }

        public void SetDefault(IEnumerable<KeyValuePair<string, string>> keyValuePairs)
        {
            ThrowIfWasStarted();

            wasProvidedWithDefaultValues = true;
            foreach (var pair in keyValuePairs)
            {
                configs[Unify(pair.Key)] = pair.Value;
            }
        }

        public async Task WaitFor(Task delayTask)
        {
            ThrowIfNotStarted();

            if (IsLoaded)
            {
                Debug.Log("Configs are already loaded, not waiting.");
                return;
            }

            var timeoutTask = delayTask.ContinueWith(OnTimeout);
            await Task.WhenAny(fetchingTask, timeoutTask).ConfigureAwait(false);
        }

        private void OnTimeout(Task task)
        {
            if (IsLoaded)
            {
                return;
            }

            Debug.LogWarning("Configs fetching timeout reached.");
            wasFetchingTimedOut = true;
        }

        public void Wait()
        {
            ThrowIfNotStarted();
            fetchingTask.Wait();
        }

        public Task WaitFetching()
        {
            ThrowIfNotStarted();
            return fetchingTask;
        }
        
        public IConfigProvider GetProvider()
        {
            throw new NotImplementedException();
        }

        public string GetConfigOf(string key)
        {
            ThrowIfNotLoaded();

            var invariantKey = Unify(key);
            if (!configs.ContainsKey(invariantKey))
            {
                throw new KeyNotFoundException(
                    $"Config of {invariantKey} wasn't found. Available configs are: {GetAllConfigKeysInAString()}");
            }

            return configs[invariantKey];
        }

        private string GetAllConfigKeysInAString()
        {
            return GetAllConfigPairs().Select(pair => pair.Key).OrderBy(s => s).Aggregate("", (s, s1) => s + ", " + s1);
        }

        public string GetMultiConfigOf(string key)
        {
            ThrowIfNotLoaded();

            var interestingKeys = configs.Keys.Where(configKey => configKey.StartsWith(Unify(key)));
            StringBuilder result = new StringBuilder("[");
            bool anyInterestingKeys = false;
            foreach (var interestingKey in interestingKeys)
            {
                var value = configs[interestingKey];
                if (string.IsNullOrWhiteSpace(value))
                {
                    continue;
                }

                // Only if we add something
                anyInterestingKeys = true;

                // Removing [ and ]
                value = value.Trim();
                value = value.Remove(0, 1);
                value = value.Remove(value.Length - 1, 1);

                result.Append(value);
                result.Append(",");
            }

            if (anyInterestingKeys)
            {
                // remove last comma
                result.Remove(result.Length - 1, 1);
            }

            result.Append("]");
            return result.ToString();
        }

        public IEnumerable<KeyValuePair<string, string>> GetAllConfigPairs()
        {
            foreach (var config in configs)
            {
                yield return config;
            }
        }

        private void ThrowIfWasStarted()
        {
            if (wasStarted)
            {
                throw new Exception($"One instance of {nameof(ConfigFetcher)} should only be started once! " +
                                    $"Beware that a lot of other parts of code could be already using the values " +
                                    $"provided by the first run. Hence getting other values will lead to inconsistencies in config values.");
            }
        }

        private void ThrowIfNotLoaded()
        {
            if (IsLoaded)
            {
                return;
            }

            if (wasFetchingTimedOut)
            {
                return;
            }

            throw new Exception(
                "You should not call this method until the end of the fetching process. Use WaitFor to wait for it to finish." +
                "Providing you with default values here may lead to inconsistencies between default and server-config values!");
        }

        private void ThrowIfNotStarted()
        {
            if (fetchingTask == null)
            {
                throw new Exception("You should call StartFetching first");
            }
        }

        private static string Unify(string key)
        {
            return key.ToLowerInvariant();
        }
    }
}