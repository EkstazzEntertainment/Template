namespace Ekstazz.Tools
{
    using UnityEngine;

    public class LoggerDisabler : MonoBehaviour
    {
        private void Awake()
        {
#if !DEBUG
            Debug.LogWarning(
                "All debug logs will be disabled after this line in release build. You can enable logs in release builds in the LoggerDisabler script");
            Debug.unityLogger.logEnabled = false;
#endif
        }
    }
}