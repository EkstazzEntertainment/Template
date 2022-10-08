namespace Ekstazz.Configs
{
    using System.Reflection;
    
    public interface IConfigPostProcessor
    {
        void Process(object container, PropertyInfo pi);
    }
}