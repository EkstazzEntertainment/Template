namespace Ekstazz.Core.Modules
{
    using System.Collections.Generic;
    using System.Linq;

    
    public class ContextInstallersHolder
    {
        public IEnumerable<IModuleInitializer> ModuleInitializers => initializers.ToList();
        
        private readonly List<IModuleInitializer> initializers;
        
        
        public ContextInstallersHolder(List<IModuleInstaller> addons)
        {
            initializers = addons
                .Select(installer => installer.ModuleInitializer)
                .Where(installer => installer != null)
                .ToList();
        }
    }
}
