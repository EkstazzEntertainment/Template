namespace Ekstazz.ProtoGames.Cameras
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "CameraShake", menuName = "Chopper/CameraShake")]
    public class CameraShake : ScriptableObject
    {
        public CameraShakeType type;
        public CameraNoise noise;
        public float transitionTime = 0.3f;
        public float duration = 0.5f;
    }

    public enum CameraShakeType
    {
        Light,
        Heavy
    }
}