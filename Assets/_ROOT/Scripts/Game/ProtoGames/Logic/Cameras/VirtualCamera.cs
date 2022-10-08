namespace Ekstazz.ProtoGames.Cameras
{
    using System;
    using System.Collections;
    using Cinemachine;
    using DG.Tweening;
    using UnityEngine;

    
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class VirtualCamera : MonoBehaviour
    {
        [Header("Setup")]
        [SerializeField] private TypeOfCamera type;
        [SerializeField] protected CinemachineVirtualCamera cm;

        [SerializeField] protected bool isTransPoser = true;
        [SerializeField] protected bool isComposer = true;
        
        public CinemachineVirtualCamera Camera => cm;
        public CinemachineTransposer TransPoser => isTransPoser ? cm.GetCinemachineComponent<CinemachineTransposer>() : null;
        public CinemachineComposer Composer => isComposer ? cm.GetCinemachineComponent<CinemachineComposer>() : null;
        public TypeOfCamera TypeOfCamera => type;
        public bool IsActive => gameObject.activeSelf;

        public bool IsShaking { get; private set; }
        public bool IsZooming { get; private set; }
        
        private Coroutine noiseCoroutine;
        private Coroutine zoomCoroutine;

        private event Action<CameraOverrideInfo> OnCameraInfoChanged; 

        
        public void InitCamera(Action<CameraOverrideInfo> camInfo)
        {
            OnCameraInfoChanged = camInfo;
        }

        public void SaveCurrentConfigurations()
        {
            var info = new CameraOverrideInfo();
            info.cameraType = TypeOfCamera;
            info.camPos = isTransPoser ? TransPoser.m_FollowOffset : Vector3.zero;
            info.camRot = isComposer ? Composer.m_TrackedObjectOffset : Vector3.zero;
            info.fieldOfView = Camera.m_Lens.FieldOfView;
            OnCameraInfoChanged?.Invoke(info);
        }

        public virtual void SetActive(bool isActive)
        {
            if (IsActive != isActive)
            {
                SetActiveInternal(isActive);
            }
        }

        protected virtual void SetActiveInternal(bool isActive)
        {
            cm.gameObject.SetActive(isActive);
            if (noiseCoroutine != null)
            {
                StopCoroutine(noiseCoroutine);
                SetShake(null);
            }

            if (zoomCoroutine != null)
            {
                StopCoroutine(zoomCoroutine);
                SetZoom(null, null);
            }
        }
        
        public void LookAt(Transform target)
        {
            cm.m_LookAt = target;
        }
        
        public void Follow(Transform target)
        {
            cm.m_Follow = target;
        }
        
        public void ResetLook()
        {
            cm.m_Follow = cm.m_LookAt = null;
        }
        
        public virtual void SetShake(CameraShake shake)
        {
            var perlin = Camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            if (perlin == null)
            {
                IsShaking = false;
                return;
            }
            
            if (shake == null || shake.noise == null)
            {
                IsShaking = false;
                perlin.m_AmplitudeGain = 0;
                perlin.m_FrequencyGain = 0;
                return;
            }
            
            if (noiseCoroutine != null)
            {
                StopCoroutine(noiseCoroutine);
            }
            
            noiseCoroutine = StartCoroutine(SetNoise(perlin, shake));
        }

        private IEnumerator SetNoise(CinemachineBasicMultiChannelPerlin cameraPerlin, CameraShake shake)
        {
            var halfTime = shake.transitionTime * 0.5f;
            cameraPerlin.m_NoiseProfile = shake.noise.shakeNoise;
            IsShaking = true;
            
            DOVirtual.Float(cameraPerlin.m_AmplitudeGain, shake.noise.amplitudeGain, halfTime, v => cameraPerlin.m_AmplitudeGain = v);
            DOVirtual.Float(cameraPerlin.m_FrequencyGain, shake.noise.frequencyGain, halfTime, v => cameraPerlin.m_FrequencyGain = v);
            yield return new WaitForSeconds(halfTime);
            
            DOVirtual.Float(cameraPerlin.m_AmplitudeGain, 0, halfTime, v => cameraPerlin.m_AmplitudeGain = v);
            DOVirtual.Float(cameraPerlin.m_FrequencyGain, 0, halfTime, v => cameraPerlin.m_FrequencyGain = v);
            yield return new WaitForSeconds(halfTime);
            yield return new WaitForSeconds(shake.duration);
            IsShaking = false;
        }
        
        public virtual void SetZoom(CameraZoom zoom, Transform zoomTarget, Action zoomInFinished = null)
        {
            if (zoom == null || zoomTarget == null)
            {
                IsZooming = false;
                return;
            }
            
            if (zoomCoroutine != null)
            {
                StopCoroutine(zoomCoroutine);
            }
            
            zoomCoroutine = StartCoroutine(StartZoom(zoom, zoomTarget, zoomInFinished));
        }

        private IEnumerator StartZoom(CameraZoom zoom, Transform zoomTarget, Action zoomInFinished = null)
        {
            var halfTime = zoom.transitionTime * 0.5f;
            var previousFieldOfView = cm.m_Lens.FieldOfView;
            var previousTarget = cm.m_LookAt;

            cm.m_Lens.FieldOfView = zoom.fieldOfView;
            IsZooming = true;
            
            cm.m_LookAt = zoomTarget;
            DOVirtual.Float(
                cm.m_Lens.FieldOfView,
                zoom.fieldOfView, halfTime,
                v => cm.m_Lens.FieldOfView = v).OnComplete(() =>
            {
                zoomInFinished?.Invoke();
            });

            yield return new WaitForSeconds(halfTime);

            DOVirtual.Float(
                cm.m_Lens.FieldOfView, 
                previousFieldOfView, 
                halfTime, 
                v => cm.m_Lens.FieldOfView = v);
            yield return new WaitForSeconds(halfTime);
            yield return new WaitForSeconds(zoom.duration);
            cm.m_LookAt = previousTarget;
            IsZooming = false;
        }
        
        public void SetFollowOffset(Vector3 vector)
        {
            if (isTransPoser)
            {
                var transposer = cm.GetCinemachineComponent<CinemachineTransposer>();
                transposer.m_FollowOffset = vector;
            }
        }
        
        public void SetTrackedObjectOffset(Vector3 vector)
        {
            if (isComposer)
            {
                var composer = cm.GetCinemachineComponent<CinemachineComposer>();
                composer.m_TrackedObjectOffset = vector;
            }
        }
    }
}