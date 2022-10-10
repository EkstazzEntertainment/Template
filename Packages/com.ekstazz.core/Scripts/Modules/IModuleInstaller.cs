namespace Ekstazz.Core.Modules
{
    using Core;
    using Zenject;

    
    public interface IModuleInstaller
    {
        Installer ContextInstaller { get; }
        IModuleInitializer ModuleInitializer { get; }
        IModuleVerifier Verifier { get; }
        Priority Priority { get; }
        BuildType SupportedBuildType { get; }
        PlatformType SupportedPlatformType { get; }
        string Name { get; }
    }
}
