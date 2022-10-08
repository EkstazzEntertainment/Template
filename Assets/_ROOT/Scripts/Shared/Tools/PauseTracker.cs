namespace Ekstazz.Tools
{
    using System;
    using UnityEngine;

    
    public class PauseTracker : MonoBehaviour
    {
        public event Action<bool> ApplicationPause;
        public event Action ApplicationQuitting;

        
        private void OnApplicationPause(bool pauseStatus)
        {
            InvokeOnApplicationPause(pauseStatus);
        }

        private void OnApplicationQuit()
        {
            OnApplicationQuitting();
        }

        private void InvokeOnApplicationPause(bool isPaused)
        {
            Debug.Log($"Application paused <color=blue>{isPaused}</color>");
            ApplicationPause?.Invoke(isPaused);
        }

        private void OnApplicationQuitting()
        {
            Debug.Log($"Application quitting");
            ApplicationQuitting?.Invoke();
        }
    }
}