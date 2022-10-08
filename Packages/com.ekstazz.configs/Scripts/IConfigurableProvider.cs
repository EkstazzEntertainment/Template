namespace Ekstazz.Configs
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine.Scripting;

    public interface IConfigurableProvider
    {
        IEnumerable<object> Configurables { get; }

        void AddObjectToConfigure(object obj);
    }

    internal class ConfigurableProvider : IConfigurableProvider
    {
        private readonly List<object> configurables = new List<object>();

        public IEnumerable<object> Configurables => configurables.ToList(); 

        [Preserve]
        public ConfigurableProvider()
        {
        }

        public void AddObjectToConfigure(object obj)
        {
            if (configurables.Any(o => o.GetType() == obj.GetType()))
            {
                return;
            }
            
            configurables.Add(obj);
        }
    }
}