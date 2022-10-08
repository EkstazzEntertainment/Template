namespace Ekstazz.DebugPanel
{
    using System.Collections.Generic;
    using UnityEngine;
    using Zenject;

    public class DebugPage : MonoBehaviour
    {
        [Inject]
        private DiContainer DiContainer { get; set; }
        
        [SerializeField]
        private DebugComponentHeader headerPrefab;

        [SerializeField]
        private GameObject separationLine;

        [SerializeField]
        private Transform content;

        public void AddPanelComponents(List<DebugComponent> components)
        {
            DeletePreviousComponents();
            SetNewComponents(components);
        }

        private void DeletePreviousComponents()
        {
            foreach(Transform child in content)
            {
                Destroy(child.gameObject);
            }
        }

        private void SetNewComponents(List<DebugComponent> componentPrefabs)
        {
            foreach (var componentPrefab in componentPrefabs)
            {
                AddHeader(componentPrefab);
                AddComponent(componentPrefab);
                AddSeparationLine();
            }
        }

        private void AddHeader(DebugComponent component)
        {
            var headerName = ExtractHeader(component);
            var header = Instantiate(headerPrefab, content);
            header.SetName(headerName);
        }

        private string ExtractHeader(DebugComponent component)
        {
            return component.gameObject.name.Replace("DebugComponent", "");
        }

        private void AddComponent(DebugComponent componentPrefab)
        {
            DiContainer.InstantiatePrefabForComponent<DebugComponent>(componentPrefab, content);
        }

        private void AddSeparationLine()
        {
            Instantiate(separationLine, content);
        }
    }
}