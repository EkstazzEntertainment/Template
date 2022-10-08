namespace Ekstazz.Tools
{
    using JetBrains.Annotations;
    using UnityEngine;
    using Random = UnityEngine.Random;

    
    /// <summary>
    /// The greatest script of all times by George Isachenko
    /// </summary>
    public class WanderAround : MonoBehaviour
    {
        [SerializeField] private bool isWorking;

        public Vector3 TranslateAmplitude = new Vector3(0.2f, 0.2f, 0.2f);
        public Vector3 RotateAmplitude = new Vector3(7, 5, 7);
        public float TimeScale = 0.1f;

        public Vector3 OriginalRotation => this.startAngles;
        public Vector3 OriginalPosition => this.startPosition;

        private Vector3 startPosition;
        private Vector3 lastPosition;
        private Vector3 startAngles;
        private Vector3 lastAnlges;

        public float Offset;
        
        private Vector3 MovementNoiseVector => new Vector3(
                TranslateAmplitude.x
                * (1 - 2 * Mathf.PerlinNoise(Time.time * TimeScale + Offset, 0)),
                TranslateAmplitude.y
                * (1 - 2 * Mathf.PerlinNoise(Time.time * TimeScale + Offset, 5)),
                TranslateAmplitude.z
                * (1 - 2 * Mathf.PerlinNoise(Time.time * TimeScale + Offset, 10)));

        private Vector3 RotationNoiseVector => new Vector3(
            RotateAmplitude.x*(1 - 2*Mathf.PerlinNoise(Time.time*TimeScale + Offset, 0)),
            RotateAmplitude.y*(1 - 2*Mathf.PerlinNoise(Time.time*TimeScale + Offset, 5)),
            RotateAmplitude.z*(1 - 2*Mathf.PerlinNoise(Time.time*TimeScale + Offset, 10)));

        
        public void StopWandering()
        {
            isWorking = false;
        }

        public void StartWandering()
        {
            Awake();
            isWorking = true;
        }

        [UsedImplicitly]
        private void LateUpdate()
        {
            if (!isWorking)
            {
                return;
            }

            UpdatePosition();
            UpdateRotation();
        }

        private void UpdatePosition()
        {
            // In case object is being moved by other script
            if (lastPosition != transform.localPosition)
            {
                startPosition = transform.localPosition - MovementNoiseVector;
            }

            transform.localPosition = startPosition + MovementNoiseVector;
            lastPosition = transform.localPosition;
        }

        private void UpdateRotation()
        {
            // In case object is being moved by other script
            if (lastAnlges != transform.localRotation.eulerAngles)
            {
                startAngles = transform.localRotation.eulerAngles - RotationNoiseVector;
            }

            transform.localRotation = Quaternion.Euler(startAngles + RotationNoiseVector);
            lastAnlges = transform.localRotation.eulerAngles;
        }

        [UsedImplicitly]
        private void Awake()
        {
            Offset = Random.value*1000;

            startPosition = transform.localPosition - MovementNoiseVector;
            lastPosition = startPosition;
            startAngles = transform.localRotation.eulerAngles - RotationNoiseVector;
            lastAnlges = startAngles;
        }

        private void OnDestroy()
        {
            StopWandering();
        }
    }
}