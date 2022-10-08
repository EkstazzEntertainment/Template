namespace Ekstazz.DebugPanel
{
    using UnityEngine;

    public class DebugComponent : MonoBehaviour
    {
        [DebugTabId]
        public string tabId;

        public int priority;

        public bool needToShow = true;
    }
}