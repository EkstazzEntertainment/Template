namespace Ekstazz.DebugPanel
{
    using TMPro;
    using UnityEngine;

    public class DebugComponentHeader : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text header;

        public void SetName(string componentName)
        {
            header.SetText(componentName);
        }
    }
}