namespace Ekstazz.Utils
{
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class VersionIndicator : MonoBehaviour
    {
        [SerializeField]
        private string prefix = "v";
        
        private void Awake()
        {
            var text = GetComponent<Text>();
            if (text != null)
            {
                text.text = $"{prefix}{Application.version}";
                return;
            }
            
            var textPro = GetComponent<TMP_Text>();
            if (textPro != null)
            {
                textPro.SetText($"{prefix}{Application.version}");
            }
        }
    }
}
