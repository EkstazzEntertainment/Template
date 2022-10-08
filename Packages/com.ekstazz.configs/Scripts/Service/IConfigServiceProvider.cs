namespace Ekstazz.Configs
{
    public interface IConfigServiceProvider
    {
        string Name { get; }
        
        int Priority { get; }
        
        IConfigServiceWrapper CreateWrapper();
    }
}