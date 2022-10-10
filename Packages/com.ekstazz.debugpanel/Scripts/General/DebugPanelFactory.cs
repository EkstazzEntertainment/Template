namespace Ekstazz.DebugPanel
{
    using UnityEngine;
    using Zenject;

    
    public class DebugPanelFactoryNormal : IDebugPanelFactory
    {
        private DiContainer diContainer;
        private Object prefab;
        private DebugPanelRoot root;

        
        public DebugPanelFactoryNormal(DiContainer diContainer, Object prefab, DebugPanelRoot root)
        {
            this.diContainer = diContainer;
            this.prefab = prefab;
            this.root = root;
        }

        public DebugPanel Create()
        {
            var panel = diContainer.InstantiatePrefabForComponent<DebugPanel>(prefab, root.transform);
            MakePanelOverlayEverything(panel);
            return panel;
        }

        private void MakePanelOverlayEverything(DebugPanel panel)
        {
            panel.transform.SetAsLastSibling();
        }
    }
}