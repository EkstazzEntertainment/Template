namespace Ekstazz.ProtoGames.Cameras
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Cinemachine;
    using Settings.Camera;
    using UnityEngine;
    using Zenject;
    using Zenject.Extensions.Lazy;


    public interface IGameCamerasController
    {
        void SwitchCameraTo(TypeOfCamera type);
        void ResetCameras();
    }
    
    
    public class GameCamerasController<T> : InjectableView<T>, IGameCamerasController where T : GameCamerasController<T>
    {
        [Inject] private CameraDefaultInfosSettings cameraDefaultInfosSettings;
        
        [SerializeField] private CameraShake[] shakes;
        [SerializeField] private CameraZoom[] zooms;
        [SerializeField] private List<VirtualCamera> cameras;
        
        [HideInInspector] public Camera mainCamera;

        private List<CameraOverrideInfo> CameraDefaultInfos => cameraDefaultInfosSettings.camerasDefaultsInfosHolders
            .Single(holder => holder.gameId == SettingsPath).infos;

        protected virtual string SettingsPath { get; set; } = "";

        public TypeOfCamera CurrentTypeOfCamera { get; private set; } = TypeOfCamera.None;
        public VirtualCamera CurrentCamera { get; private set; }

        private CinemachineBrain cineMachineBrain;
        
        
        protected override void Awake()
        {
            base.Awake();
            mainCamera = Camera.main;
            cineMachineBrain = mainCamera.GetComponent<CinemachineBrain>();
            
            InitCameras();
        }

        public virtual void InitCameras()
        {
            cameras.ForEach(cam => cam.InitCamera((info) =>
            {
                CacheSavedCameraParams(info);
            }));
        }

        private void CacheSavedCameraParams(CameraOverrideInfo info)
        {
            var existing = CameraDefaultInfos.SingleOrDefault(camInfo => camInfo.cameraType == info.cameraType);
            if (existing == null)
            {
                CameraDefaultInfos.Add(info);
            }
            else
            {
                existing.camPos = info.camPos;
                existing.camRot = info.camRot;
                existing.fieldOfView = info.fieldOfView;
            }
        }

        public void SwitchCameraTo(TypeOfCamera type)
        {
            foreach (var cam in cameras)
            {
                var active = cam.TypeOfCamera == type;
                if (active)
                {
                    CurrentCamera = cam;
                }
                cam.SetActive(active);
            }
            CurrentTypeOfCamera = type;
        }

        public virtual void ResetCameras()
        {
            foreach (var cam in cameras)
            {
                var info = CameraDefaultInfos.First(camInfo => camInfo.cameraType == cam.TypeOfCamera);
                cam.SetFollowOffset(info.camPos);
                cam.SetTrackedObjectOffset(info.camRot);
                cam.Camera.m_Lens.FieldOfView = info.fieldOfView;
            }
        }

        public void TurnOffCameras()
        {
            foreach (var cam in cameras)
            {
                cam.gameObject.SetActive(false);
            }
        }

        public void ShakeCamera(CameraShakeType shakeType)
        {
            var cams = cameras.Where(c => c.IsActive && !c.IsShaking);
            var shake = shakes.FirstOrDefault(sh => sh.type == shakeType);
            if (shake == null) return;
            foreach (var cam in cams)
            {
                cam.SetShake(shake);
            }
        }

        public void ZoomCamera(CameraZoomType zoomType, Transform zoomTarget, Action zoomInFinished = null)
        {
            var cams = cameras.Where(c => c.IsActive && !c.IsZooming);
            var zoom = zooms.FirstOrDefault(sh => sh.type == zoomType);
            if (zoom == null)
            {
                return;
            }
            foreach (var cam in cams)
            {
                cam.SetZoom(zoom, zoomTarget, zoomInFinished);
            }
        }

        public VirtualCamera GetCamera(TypeOfCamera type)
        {
            return cameras.First(c => c.TypeOfCamera == type);
        }

        public void ExecuteAfterBlend(Action action)
        {
            var coroutine = ExecuteAfterBlendCoroutine(action);
            StartCoroutine(coroutine);
        }

        private IEnumerator ExecuteAfterBlendCoroutine(Action action)
        {
            yield return null;
            yield return null;
            while (cineMachineBrain.IsBlending)
            {
                yield return null;
            }
            action?.Invoke();
        }
    }
    
    
    [Serializable]
    public class CameraOverrideInfo
    {
        public TypeOfCamera cameraType;
        public Vector3 camPos = new Vector3(0, 10, -4);
        public Vector3 camRot = Vector3.zero;
        public float fieldOfView = 40;
    }
}