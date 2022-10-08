namespace Ekstazz.Sounds
{
    using System;
    using System.Collections;
    using Ekstazz.Core;
    using MoreMountains.NiceVibrations;
    using UnityEngine;

    [Serializable]
    public class Vibration
    {
        [Range(0, 1)]
        public float intensity;

        [Range(0, 1)]
        public float sharpness;
    }

    [Serializable]
    public class ContinuousVibration : Vibration
    {
        [Min(0.001f)]
        public float duration;
    }

    [Serializable]
    public class AdvancedVibration
    {
        public TextAsset AHAPFile;
        public MMNVAndroidWaveFormAsset waveFormAsset;
        public MMNVRumbleWaveFormAsset rumbleWaveFormAsset;
    }
}