namespace Ekstazz.Shared.Debug
{
    using Ekstazz.Core;
    using Ekstazz.Core.Modules;
    using Ekstazz.Debug.DebugOptions;
    using Zenject;

    
    [AutoInstalledModule]
    public class DebugInstaller : ModuleInstaller
    {
        public override string Name => "Ekstazz.Debug";
        public override BuildType SupportedBuildType => BuildType.Debug;
        public override IModuleInitializer ModuleInitializer => new Initializer();

        
        public override void InstallBindings()
        {
            Container.Bind<FpsPanelVisibilityController>().AsSingle();
            Container.Bind<DeviceInfoPanelVisibilityController>().AsSingle();
        }

        private class Initializer : IModuleInitializer
        {
            [Inject] private DebugOptionsRouter debugOptionsRouter;
            [Inject] private FpsPanelVisibilityController fpsPanelVisibilityController;
            [Inject] private DeviceInfoPanelVisibilityController deviceInfoPanelVisibilityController;

            
            public void Prepare()
            {
                debugOptionsRouter.AddOption(new ToggleOption
                {
                    Name = "Visible FPS panel",
                    Default = false,
                    OnValueChanged = fpsPanelVisibilityController.SetNewAlpha
                });

                debugOptionsRouter.AddOption(new ToggleOption
                {
                    Name = "Visible device info panel",
                    Default = false,
                    OnValueChanged = deviceInfoPanelVisibilityController.SetNewAlpha
                });
            }
        }
    }
}
