namespace Ekstazz.Ui
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    
    [RequireComponent(typeof(Image)), RequireComponent(typeof(Button))]
    public class UiBackground : MonoBehaviour
    {
        private Button button;
        private Image image;
        private bool clicked;

        public Action ClickAction { get; set; }

        
        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(Click);
            image = GetComponent<Image>();
        }

        public void MakeTransparent()
        {
            image.color = Color.clear;
        }

        private void Click()
        {
            if (clicked)
            {
                return;
            }

            clicked = true;
            ClickAction?.Invoke();
        }
    }
}
