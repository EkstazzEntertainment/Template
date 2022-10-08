namespace Ekstazz.Configs
{
    using System.Reflection;
    
    public class EmptyPostProcessor : IConfigPostProcessor
    {
        public void Process(object container, PropertyInfo pi)
        {
        }
    }
}