namespace Ekstazz.DebugPanel
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using Zenject;

    public class DebugPanel : DebugViewParameter<float>
    {
        [Inject]
        public DebugPanelAlphaController DebugPanelAlphaController { get; set; }

        [Inject]
        public DebugTabSettings DebugTabSettings { get; set; }
        
        [Inject]
        public LastTabIndexProvider LastTabIndexProvider { get; set; }

        [SerializeField]
        private List<DebugTab> tabs;

        [SerializeField]
        private DebugPage debugPage;

        [SerializeField]
        private CanvasGroup canvasGroup;

        private DebugTab currentTab;
        private List<string> ids;
        private DebugComponent[] components;

        public override void ApplyValue(float value)
        {
            canvasGroup.alpha = value;
        }

        private void Start()
        {
            InitializeTabs();
            DebugPanelAlphaController.SetInstance(this);
        }

        private void OnValidate()
        {
            tabs = GetComponentsInChildren<DebugTab>().ToList();
        }

        private void InitializeTabs()
        {
            LoadAllPanelComponents();
            GetTabIds();
            SetupTabs();
            SetupComponentsWithoutId();
            ActivateLastOpenedTab();
        }

        private void LoadAllPanelComponents()
        {
            components = Resources.LoadAll<DebugComponent>("DebugPanel");
        }

        private void GetTabIds()
        {
            ids = DebugTabSettings.ids;
        }

        private void SetupTabs()
        {
            for (var i = 0; i < ids.Count; i++)
            {
                var tab = tabs[i];
                var id = ids[i];
                SetupTab(tab, id);
            }
        }

        private void SetupTab(DebugTab tab, string id)
        {
            tab.TabLabel.text = id;
            tab.components = GetPanelComponentsBy(id);
            tab.TabButton.onClick.AddListener(() => ActivateTab(tab));
            tab.SetActive(false);
        }

        private List<DebugComponent> GetPanelComponentsBy(string id)
        {
            return components
                .Where(c => c.tabId == id && c.needToShow)
                .OrderBy(c => c.priority)
                .ToList();
        }

        private void ActivateLastOpenedTab()
        {
            var index = LastTabIndexProvider.GetLastIndex();
            var tab = tabs[index];
            ActivateTab(tab);
        }

        private void ActivateTab(DebugTab newTab)
        {
            if (currentTab != null)
            {
                currentTab.SetActive(false);
            }

            ActivateNewTab(newTab);
            UpdateLastIndex();
        }

        private void ActivateNewTab(DebugTab newTab)
        {
            newTab.SetActive(true);
            currentTab = newTab;
            debugPage.AddPanelComponents(newTab.components);
        }

        private void UpdateLastIndex()
        {
            var index = tabs.IndexOf(currentTab);
            LastTabIndexProvider.SetLastIndex(index);
        }

        private void SetupComponentsWithoutId()
        {
            var lastTab = tabs.Last();
            var debugComponents = GetComponentsWithWrongId();
            lastTab.components.AddRange(debugComponents);
        }

        private IEnumerable<DebugComponent> GetComponentsWithWrongId()
        {
            return components
                .Where(c => !ids.Contains(c.tabId))
                .OrderBy(c => c.priority)
                .ToList();
        }

        public void Close()
        {
            Destroy(gameObject);
        }
        
        public class Factory : PlaceholderFactory<Component, DebugPanel>
        {
        }
    }
}