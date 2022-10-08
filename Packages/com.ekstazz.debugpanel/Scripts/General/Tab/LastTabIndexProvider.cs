namespace Ekstazz.DebugPanel
{
    using UnityEngine;

    public class LastTabIndexProvider
    {
        private const string Key = "DebugLastTabIndex";

        public int GetLastIndex()
        {
            return PlayerPrefs.GetInt(Key, 0);
        }

        public void SetLastIndex(int value)
        {
            PlayerPrefs.SetInt(Key, value);
        }
    }
}