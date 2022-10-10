namespace Ekstazz.Configs.Flow
{
    using System.Threading.Tasks;
    using Cache;
    using Zenject;
    using Zenject.Extensions.Commands;

    
    public class InitConfigsFetchingCommand : Command
    {
        [Inject(Id = FetcherOrigin.Local)]
        internal IConfigFetcher LocalFetcher { get; set; }

        [Inject(Id = FetcherOrigin.Remote)]
        internal IConfigFetcher RemoteFetcher { get; set; }

        [Inject]
        internal ICacheSaver CacheSaver { get; set; }

        
        public override async Task Execute()
        {
            CacheSaver.Load().Wait();

            // Block execution before all local configs are loaded
            LocalFetcher.StartFetching();
            LocalFetcher.Wait();

            // Use them as defaults to remote configs
            RemoteFetcher.SetDefault(LocalFetcher.GetAllConfigPairs());

            RemoteFetcher.StartFetching();
        }
    }
}