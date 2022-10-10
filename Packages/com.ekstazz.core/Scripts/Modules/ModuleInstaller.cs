namespace Ekstazz.Core.Modules
{
    using System.Collections.Generic;
    using System.Linq;
    using Core;
    using Zenject;

    
    public abstract class ModuleInstaller : Installer, IModuleInstaller, IModuleVerifier
    {
        public Installer ContextInstaller => this;
        public IModuleVerifier Verifier => this;
        public virtual IModuleInitializer ModuleInitializer => null;
        public virtual BuildType SupportedBuildType => BuildType.Any;
        public virtual PlatformType SupportedPlatformType => PlatformType.Any;
        public virtual Priority Priority => Priority.Simple;
        public abstract string Name { get; }

        
        public ModuleVerificationResult Verify()
        {
            var messages = FindModuleErrors()
                .SelectMany(result => result.Messages)
                .ToList();
            return messages.Any() ? ModuleVerificationResult.FromMessages(messages) : ModuleVerificationResult.Valid;
        }

        protected virtual IEnumerable<ModuleVerificationResult> FindModuleErrors()
        {
            yield return ModuleVerificationResult.Valid;
        }
    }
}
