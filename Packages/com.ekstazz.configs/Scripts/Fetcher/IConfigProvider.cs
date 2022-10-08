namespace Ekstazz.Configs
{
    internal interface IConfigProvider
    {
        string GetConfigOf(string key);
        string GetMultiConfigOf(string key);
    }
}