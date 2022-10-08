namespace Ekstazz.DebugPanel
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    [CreateAssetMenu(fileName = nameof(DebugTabSettings), menuName = "Ekstazz/DebugTabSettings")]
    public class DebugTabSettings : ScriptableObject
    {
        private const int MaxSize = 4;
        public List<string> ids;

        public void OnValidate()
        {
            if (ids.Count != MaxSize)
            {
                ids = ids.Take(MaxSize).ToList();
            }
        }
    }
}
