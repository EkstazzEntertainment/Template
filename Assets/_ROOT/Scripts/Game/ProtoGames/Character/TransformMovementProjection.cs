namespace Ekstazz.ProtoGames.Character
{
    using UnityEngine;

    public class TransformMovementProjection : MonoBehaviour
    {
        [Header("Setup")]
        [SerializeField] private Transform trackedTransform;
        
        [Header("Projection settings")]
        [SerializeField, Range(0, 1f)] private float sensitivity = 0.7f;

        
        public Transform TrackedTransform
        {
            get => trackedTransform;
            set => trackedTransform = value;
        }
        
        private void Awake()
        {
            if (trackedTransform == null)
            {
                trackedTransform = transform;
            }
        }

        private void FixedUpdate()
        {
            transform.position = Vector3Extensions.CosineInterpolate(transform.position,GetProjectionOnPath(TrackedTransform.position), sensitivity);
        }

        private Vector3 GetProjectionOnPath(Vector3 sourcePosition)
        {
            return sourcePosition;
        }
    }
    
    public static class Vector3Extensions
    {
        public static Vector3 CosineInterpolate(Vector3 a, Vector3 b, float t)
        {
            t = Mathf.Clamp01(t);
            var t2 = (1 - Mathf.Cos(t * Mathf.PI)) / 2;
            return a * (1 - t2) + b * t2;
        }
    }
}