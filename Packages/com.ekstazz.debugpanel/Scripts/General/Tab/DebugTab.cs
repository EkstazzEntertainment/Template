namespace Ekstazz.DebugPanel
{
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class DebugTab : MonoBehaviour
    {
        [field: SerializeField]
        public Button TabButton { get; private set; }

        [field: SerializeField]
        public TMP_Text TabLabel { get; private set; }

        [SerializeField]
        private Image background;

        [SerializeField]
        private Color activeColor;

        [SerializeField]
        private Color disabledColor;

        public List<DebugComponent> components;

        public void SetActive(bool value)
        {
            background.color = value ? activeColor : disabledColor;
            TabLabel.color = value ? disabledColor : activeColor;
        }
    }
}