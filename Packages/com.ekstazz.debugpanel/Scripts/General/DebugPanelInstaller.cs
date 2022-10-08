namespace Ekstazz.DebugPanel
{
    using Core.Modules;
    using Debug.DebugOptions;
    using UnityEngine;
    using Zenject;
    using Zenject.Extensions.Commands;

    [AutoInstalledModule]
    public class DebugPanelInstaller : ModuleInstaller
    {
        public override IModuleInitializer ModuleInitializer => new Initializer();
        public override string Name => "Ekstazz.DebugPanel";

        public override void InstallBindings()
        {
            Container.Bind<RuntimeOptions>().AsSingle();
            Container.Bind<ToggleOptions>().AsSingle();
            Container.Bind<InvokableOptions>().AsSingle();
            Container.Bind<DebugPanelAlphaController>().AsSingle();
            Container.Bind<DebugButtonVisibilityController>().AsSingle();
            Container.Bind<LastTabIndexProvider>().AsSingle();
            Container.Bind<IDebugPanelFactory>().To<DebugPanelFactoryProxy>().AsSingle();
            Container.Bind<DebugTabSettings>().FromResourceSettings();
            Container.Bind<DebugOptionsRouter>().AsSingle();
        }

        public class Initializer : IModuleInitializer
        {
            [Inject]
            private DebugOptionsRouter DebugOptionsRouter { get; set; }

            [Inject]
            private DebugPanelAlphaController DebugPanelAlphaController { get; set; }

            [Inject]
            private DebugButtonVisibilityController DebugButtonVisibilityController { get; set; }

            public void Prepare()
            {
                DebugOptionsRouter.AddOption(new RuntimeOption
                {
                    Name = "Time Scale",
                    Min = 0,
                    Max = 3,
                    Default = 1,
                    OnValueChanged = f => Time.timeScale = f
                });

                DebugOptionsRouter.AddOption(new RuntimeOption
                {
                    Name = "Panel alpha",
                    Min = 0.01f,
                    Max = 1f,
                    Default = 1,
                    OnValueChanged = DebugPanelAlphaController.SetNewAlpha
                });
                
                DebugOptionsRouter.AddOption(new ToggleOption
                {
                    Name = "Visible debug button",
                    Default = true,
                    OnValueChanged = DebugButtonVisibilityController.SetNewAlpha
                });
            }
        }
    }
}