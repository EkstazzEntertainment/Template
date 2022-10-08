namespace Ekstazz.LevelBased.Debug
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Ekstazz.LevelBased.Logic;
    using Newtonsoft.Json;
    using UnityEngine;
    using Zenject;

    
    public class LevelConfigDebug : MonoBehaviour
    {
        [Inject] private ILevelProvider levelProvider;

        [SerializeField] private LevelConfigElement levelConfigElementPrefab;
        [SerializeField] private RectTransform panelRect;

        private LevelConfig config;
        private PropertyInfo[] fields;
        private readonly List<LevelConfigElement> elements = new List<LevelConfigElement>();

        
        private void Start()
        {
            LoadConfigAndFields();
            CreateConfigLines();
            ResizeView();
        }

        private void LoadConfigAndFields()
        {
            config = levelProvider.CurrentLevel.Config;
            fields = config.GetType().GetProperties();
        }

        private void CreateConfigLines()
        {
            foreach (var field in fields)
            {
                var text = GetElementText(field);
                CreateConfigElement(text);
            }
        }

        private string GetElementText(PropertyInfo property)
        {
            var value = property.GetValue(config);
            var formattedValue = JsonConvert.SerializeObject(value, Formatting.Indented);
            return $"{property.Name}: <b>{formattedValue}</b>";
        }

        private void CreateConfigElement(string text)
        {
            var element = Instantiate(levelConfigElementPrefab, transform);
            element.Setup(text);
            elements.Add(element);
        }

        private void ResizeView()
        {
            var height = CalculateNewHeight();
            SetRectHeight(height);
        }

        private float CalculateNewHeight()
        {
            return elements.Sum(element => element.CalculatedHeight);
        }

        private void SetRectHeight(float totalHeight)
        {
            var size = panelRect.sizeDelta;
            size.y += totalHeight;
            panelRect.sizeDelta = size;
        }
    }
}