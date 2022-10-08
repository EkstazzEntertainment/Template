namespace Ekstazz.Wallet.Editor
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Ekstazz.Currencies;
    using UnityEditor;
    using UnityEngine;

    
    [CustomPropertyDrawer(typeof(CurrencyAttribute))]
    public class CurrencyDrawer : PropertyDrawer
    {
        private readonly string[] ids;
        private int index;

        public CurrencyDrawer()
        {
            ids = LoadTabIds();
        }

        private string[] LoadTabIds()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .Where(ShouldCheck)
                .SelectMany(assembly => assembly.GetExportedTypes())
                .Where(t => !t.IsInterface && !t.IsAbstract && typeof(ICurrencyType).IsAssignableFrom(t))
                .Select(t => (ICurrencyType)Activator.CreateInstance(t))
                .Select(c => c.Name)
                .ToArray();


            bool ShouldCheck(Assembly assembly)
            {
                var name = assembly.FullName;
                return name.Contains("Ekstazz") || name.Contains("Ekstazz") || name.Contains("Game.");
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            CalculateIndex(property.stringValue);
            CreatePopup(position, property);
        }

        private void CalculateIndex(string currentValue)
        {
            index = Mathf.Max(0, Array.IndexOf(ids, currentValue));
        }

        private void CreatePopup(Rect position, SerializedProperty property)
        {
            index = EditorGUI.Popup(position, property.displayName, index, ids);
            property.stringValue = ids[index];
        }
    }
}
