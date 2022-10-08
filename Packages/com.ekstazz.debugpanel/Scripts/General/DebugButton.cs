namespace Ekstazz.DebugPanel
{
    using JetBrains.Annotations;
    using UnityEngine;
    using Zenject;

    public interface IDebugPanelFactory
    {
        DebugPanel Create();
    }
    
    public class DebugButton : DebugViewParameter<bool>
    {
        [Inject]
        public IDebugPanelFactory DebugPanelFactory { get; set; }
        
        [Inject]
        public DebugButtonVisibilityController DebugButtonVisibilityController { get; set; }

        [SerializeField]
        private CanvasGroup canvasGroup;
        
        private DebugPanel panel;

        private void Awake()
        {
#if !DEBUG
            Destroy(gameObject);
#endif
        }

        private void Start()
        {
            DebugButtonVisibilityController.SetInstance(this);
        }

        public override void ApplyValue(bool value)
        {
            canvasGroup.alpha = value ? 1 : 0;
        }

        [UsedImplicitly]
        public void ShowDebugPanel()
        {
            if (panel)
            {
                return;
            }

            SetupPanel();
        }

        private void SetupPanel()
        {
            panel = DebugPanelFactory.Create();
        }
    }
}