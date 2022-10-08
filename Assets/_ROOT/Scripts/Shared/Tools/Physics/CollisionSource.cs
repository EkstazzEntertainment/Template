namespace Ekstazz.Tools
{
    using System;
    using UnityEngine;

    
    [RequireComponent(typeof(Collider))]
    public class CollisionSource : MonoBehaviour
    {
        public Action<Collision> OnEnter;
        public Action<Collision> OnStay;
        public Action<Collision> OnExit;
        
        
        private void OnCollisionEnter(Collision other)
        {
            OnEnter?.Invoke(other);
        }

        private void OnCollisionStay(Collision other)
        {
            OnStay?.Invoke(other);
        }

        private void OnCollisionExit(Collision other)
        {
            OnExit?.Invoke(other);
        }
    }
}