namespace Ekstazz.Utils.Editor
{
    using UnityEditor;
    using UnityEngine;

    [CustomPropertyDrawer(typeof(MinMaxSliderAttribute))]
    public class MinMaxSliderDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.Vector2)
            {
                var range = property.vector2Value;
                var min = range.x;
                var max = range.y;
                var attr = attribute as MinMaxSliderAttribute;
                EditorGUI.BeginChangeCheck();
                EditorGUI.MinMaxSlider(position, label, ref min, ref max, attr.min, attr.max);
                position.y += 12;
                EditorGUI.LabelField(position, $"Values: ({min:F2}) - ({max:F2})");
                if (EditorGUI.EndChangeCheck())
                {
                    range.x = min;
                    range.y = max;
                    property.vector2Value = range;
                }
            }
            else
            {
                EditorGUI.LabelField(position, label, "Use only with Vector2");
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return property.propertyType == SerializedPropertyType.Vector2 ? 36 : 18;
        }
    }
}
