namespace Ekstazz.Shared.Debug
{
    using Ekstazz.DebugPanel;
    using UnityEngine;
    using Zenject;

    
    public class DeviceInfoPanel : DebugViewParameter<bool>
    {
        [InjectOptional] private DeviceInfoPanelVisibilityController deviceInfoPanelVisibilityController;

        [SerializeField] private GameObject deviceInfoPanelModule;

        
        private void Awake()
        {
#if !DEBUG
            Destroy(gameObject);
#else
            deviceInfoPanelVisibilityController?.SetInstance(this);
#endif
        }

        public override void ApplyValue(bool value)
        {
             deviceInfoPanelModule.SetActive(value);
        }
    }
}
