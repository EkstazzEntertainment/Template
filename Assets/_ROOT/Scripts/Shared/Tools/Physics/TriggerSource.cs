namespace Ekstazz.Tools
{
    using System;
    using UnityEngine;

    
    [RequireComponent(typeof(Collider))]
    public class TriggerSource : MonoBehaviour
    {
        public Action<Collider> OnEnter;
        public Action<Collider> OnStay;
        public Action<Collider> OnExit;

        
        private void OnTriggerEnter(Collider other)
        {
            OnEnter?.Invoke(other);
        }

        private void OnTriggerStay(Collider other)
        {
            OnStay?.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            OnExit?.Invoke(other);
        }
    }
}