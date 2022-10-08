namespace Ekstazz.Debug
{
    using System;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class NamedButton : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text namePlaceholder;
        
        [SerializeField]
        private Button button;

        private Action clickCallback;
        
        public void InitWith(string buttonName, Action callback)
        {
            namePlaceholder.SetText(buttonName);
            clickCallback = callback;
            
            button.onClick.AddListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            clickCallback?.Invoke();
        }
    }
}