namespace Ekstazz.Sounds
{
    using System;
    using MoreMountains.NiceVibrations;
    using UnityEngine;

    public class SoundSettings : ScriptableObject
    {
        [Header("Default settings")]
        [SerializeField]
        private SoundsAvailabilitySettings iosDefaultSettings;

        [SerializeField]
        private SoundsAvailabilitySettings androidDefaultSettings;

#if UNITY_ANDROID
        private SoundsAvailabilitySettings CurrentPlatformDefaultSettings => androidDefaultSettings;
#else
        private SoundsAvailabilitySettings CurrentPlatformDefaultSettings => iosDefaultSettings;
#endif

        public bool MusicEnabled => CurrentPlatformDefaultSettings.musicEnabled;

        public bool SoundsEnabled => CurrentPlatformDefaultSettings.soundEnabled;

        public bool VibrationsEnabled => CurrentPlatformDefaultSettings.vibrationEnabled;

        [Header("Master Volume")]
        [Range(0, 1)]
        public float masterMusicVolume = 1;

        [Range(0, 1)]
        public float masterSoundsVolume = 1;

        [Header("Music")]
        public Music music;

        [Header("Tap")]
        public Audio tap;

        public HapticTypes tapVibro;
    }

    [Serializable]
    public class SoundsAvailabilitySettings
    {
        public bool musicEnabled;
        public bool soundEnabled;
        public bool vibrationEnabled;
    }
}
