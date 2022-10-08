namespace Ekstazz.ProtoGames.Settings.Camera
{
    using System;
    using System.Collections.Generic;
    using Cameras;
    using UnityEngine;

    
    [CreateAssetMenu(fileName = "CameraDefaultInfosSettings", menuName = "ProtoGames/Settings/CameraDefaultInfosSettings")]
    public class CameraDefaultInfosSettings : ScriptableObject
    {
        public List<CamerasDefaultsInfosHolder> camerasDefaultsInfosHolders;
    }

    [Serializable]
    public class CamerasDefaultsInfosHolder
    {
        public string gameId;
        public List<CameraOverrideInfo> infos;
    }
}
