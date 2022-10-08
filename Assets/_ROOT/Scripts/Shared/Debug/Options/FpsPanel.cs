namespace Ekstazz.Shared.Debug
{
    using System.Collections.Generic;
    using Ekstazz.DebugPanel;
    using UnityEngine;
    using Zenject;

    
    public class FpsPanel : DebugViewParameter<bool>
    {
        [InjectOptional] private FpsPanelVisibilityController fpsPanelVisibilityController;

        [SerializeField] private List<GameObject> modules;

        
        private void Awake()
        {
#if !DEBUG
            Destroy(gameObject);
#else
            fpsPanelVisibilityController?.SetInstance(this);
#endif
        }

        public override void ApplyValue(bool value)
        {
            modules.ForEach(m => m.SetActive(value));
        }
    }
}
