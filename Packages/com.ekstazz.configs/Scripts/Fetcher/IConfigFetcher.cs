namespace Ekstazz.Configs
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    
    internal interface IConfigFetcher : IConfigProvider
    {
        bool IsLoaded { get; }
        IConfigServiceWrapper ServiceWrapper { get; set; }
        void StartFetching();
        void SetDefault(IEnumerable<KeyValuePair<string, string>> keyValuePairs);
        Task WaitFor(Task delayTask);
        void Wait();
        Task WaitFetching();
        IConfigProvider GetProvider();
        IEnumerable<KeyValuePair<string, string>> GetAllConfigPairs();
    }
}