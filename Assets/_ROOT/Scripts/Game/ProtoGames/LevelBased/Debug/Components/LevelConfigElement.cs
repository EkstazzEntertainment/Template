namespace Ekstazz.LevelBased.Debug
{
    using TMPro;
    using UnityEngine;

    public class LevelConfigElement : MonoBehaviour
    {
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private float lineHeight;
        [SerializeField] private TMP_Text configInscription;
        
        public float CalculatedHeight { get; private set; }

        
        public void Setup(string text)
        {
            SetText(text);
            CalculatedHeight = CalculateHeight(text);
            ChangeSize();
        }

        private void SetText(string value)
        {
            configInscription.text = value;
        }

        private void ChangeSize()
        {
            var rect = rectTransform.rect;
            rectTransform.sizeDelta = new Vector2(rect.width, CalculatedHeight);
        }

        private float CalculateHeight(string text)
        {
            var linesAmount = text.Split('\n').Length;
            var height = linesAmount * lineHeight;
            return height;
        }
    }
}