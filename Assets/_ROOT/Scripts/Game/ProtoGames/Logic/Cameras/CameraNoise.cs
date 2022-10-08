namespace Ekstazz.ProtoGames.Cameras
{
    using Cinemachine;
    using UnityEngine;

    [CreateAssetMenu(fileName = "CameraNoise", menuName = "Chopper/CameraNoise")]
    public class CameraNoise : ScriptableObject
    {
        public float amplitudeGain;
        public float frequencyGain;
        public NoiseSettings shakeNoise;
    }
}