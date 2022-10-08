namespace Ekstazz.DebugPanel
{
    using UnityEngine;
    using Zenject;

    public class DebugPanelFactoryProxy : IDebugPanelFactory
    {
        private DebugPanel debugPanelPrefab;
        private DebugPanelRoot debugPanelRoot;
        private readonly DebugPanelFactoryNormal proxiedFactory;

        public DebugPanelFactoryProxy(DiContainer diContainer)
        {
            debugPanelPrefab = LoadPrefab();
            debugPanelRoot = GetPanelRoot();
            proxiedFactory = new DebugPanelFactoryNormal(diContainer, debugPanelPrefab, debugPanelRoot);
        }

        public DebugPanel Create()
        {
            return proxiedFactory.Create();
        }

        private DebugPanel LoadPrefab()
        {
            debugPanelPrefab = Resources.Load<DebugPanel>("Debug Panel");
            return debugPanelPrefab;
        }

        private DebugPanelRoot GetPanelRoot()
        {
            debugPanelRoot = Object.FindObjectOfType<DebugPanelRoot>();
            return debugPanelRoot;
        }
    }
}