namespace Ekstazz.ProtoGames.Cameras
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "CameraZoom", menuName = "Chopper/CameraZoom")]
    public class CameraZoom : ScriptableObject
    {
        public CameraZoomType type;
        public float fieldOfView = 40;
        public float dutch;
        public float transitionTime = 0.3f;
        public float duration = 0.5f;
    }
    
    public enum CameraZoomType
    {
        Light,
        Heavy
    }
}