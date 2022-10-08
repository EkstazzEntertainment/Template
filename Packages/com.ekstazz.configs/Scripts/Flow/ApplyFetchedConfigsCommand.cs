namespace Ekstazz.Configs.Flow
{
    using System;
    using System.Threading.Tasks;
    using Cache;
    using UnityEngine;
    using Zenject;
    using Zenject.Extensions.Commands;

    public class ApplyFetchedConfigsCommand : Command
    {
        [Inject(Id = FetcherOrigin.Remote)]
        internal IConfigFetcher RemoteFetcher { get; set; }

        [Inject]
        internal ConfigTypeFiller ConfigTypeFiller { get; set; }

        [Inject]
        internal IConfigurableProvider ConfigurableProvider { get; set; }

        [Inject]
        internal ICacheSaver CacheSaver { get; set; }

        [Inject]
        internal IConfigCache ConfigCache { get; set; }

        public override async Task Execute()
        {
            var areConfigsLoaded = false;
            try
            {
                // Wait for fetcher to finish its job for 5 more seconds
                var waitingTask = RemoteFetcher.WaitFor(Task.Delay(5000));
                await waitingTask;
                areConfigsLoaded = RemoteFetcher.IsLoaded;

                foreach (var configurable in ConfigurableProvider.Configurables)
                {
                    ConfigTypeFiller.Fill(configurable);
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            finally
            {
                UpdateCache(areConfigsLoaded);
            }
        }

        private async void UpdateCache(bool areConfigsLoaded)
        {
            if (areConfigsLoaded)
            {
                CacheSaver.Save();
                return;
            }

            await RemoteFetcher.WaitFetching();
            foreach (var key in RemoteFetcher.ServiceWrapper.GetKeys(string.Empty))
            {
                ConfigCache.GetOrUpdateConfig(key, false);
            }

            CacheSaver.Save();
        }
    }
}