namespace Ekstazz.Configs
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IConfigServiceWrapper
    {
        Task FetchAsync();
        bool ApplyFetched();
        IEnumerable<string> GetKeys(string configNamespace);
        IConfigValueWrapper GetValue(string key, string configNamespace);
        void LogExceptionDetails(AggregateException exception);
    }
}