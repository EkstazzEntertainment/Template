namespace Ekstazz.Game.Flow
{
    using System.Collections;
    using System.Linq;
    using TMPro;
    using UnityEngine;

    public class LoadingAnimation : MonoBehaviour
    {
        [SerializeField] private TMP_Text loadingText;
        [SerializeField] [Range(0.1f, 1f)] private float animationDelay = 0.5f;
        [SerializeField] private int maxDotsCount = 2;
        
        private const string InvisibleDot = "<color=#00000000>.</color>";
        private string baseText;

        
        private void Awake()
        {
            InitWithInvisibleDots();
            StartCoroutine(Animate());
        }

        private void InitWithInvisibleDots()
        {
            baseText = loadingText.text;
            var currentText = GenerateDotsString(0);
            loadingText.text = currentText;
        }
        
        private IEnumerator Animate()
        {
            var currentDotsCount = 0;

            var waitForSeconds = new WaitForSeconds(animationDelay);
            while (true)
            {
                if (currentDotsCount > maxDotsCount) currentDotsCount = 0;
                var generatedString = GenerateDotsString(currentDotsCount);
                loadingText.text = generatedString;
                currentDotsCount++;
                yield return waitForSeconds;
            }
        }

        private string GenerateDotsString(int dotsCount)
        {
            var invisibleDotsCount = maxDotsCount - dotsCount;
            var visibleDotsStr = new string('.', dotsCount);
            var invisibleDotsStr = string.Concat(Enumerable.Repeat(InvisibleDot, invisibleDotsCount));
            return $"{baseText}{visibleDotsStr}{invisibleDotsStr}";
        }
    }
}