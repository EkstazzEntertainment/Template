namespace Ekstazz.Utils
{
    using System;
    using UnityEngine;

    
    public interface IApplicationEventsTracker
    {
        public event Action<bool> ApplicationPause;
        public event Action ApplicationQuitting;
        public event Action<bool> ApplicationFocus;
    }

    
    public class ApplicationEventsTracker : MonoBehaviour, IApplicationEventsTracker
    {
        public event Action<bool> ApplicationPause;
        public event Action ApplicationQuitting;
        public event Action<bool> ApplicationFocus;

        
        private void OnApplicationPause(bool pauseStatus)
        {
            InvokeOnApplicationPause(pauseStatus);
        }

        private void InvokeOnApplicationPause(bool isPaused)
        {
            Debug.Log($"Application paused <color=blue>{isPaused}</color>");
            ApplicationPause?.Invoke(isPaused);
        }

        private void OnApplicationQuit()
        {
            OnApplicationQuitting();
        }

        private void OnApplicationQuitting()
        {
            Debug.Log($"Application quitting");
            ApplicationQuitting?.Invoke();
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            InvokeOnApplicationFocus(hasFocus);
        }

        private void InvokeOnApplicationFocus(bool hasFocus)
        {
            Debug.Log($"Application has focus {hasFocus}");
            ApplicationFocus?.Invoke(hasFocus);
        }
    }
}