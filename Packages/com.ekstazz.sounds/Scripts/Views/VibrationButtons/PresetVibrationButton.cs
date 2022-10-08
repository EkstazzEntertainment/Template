namespace Ekstazz.Sounds
{
    using MoreMountains.NiceVibrations;
    using UnityEngine;

    public class PresetVibrationButton : TapVibrationButton
    {
        [Header("Vibration settings")]
        [SerializeField]
        private HapticTypes vibrationType;

        public override void Vibrate()
        {
            GameSounds.Haptic(vibrationType);
        }
    }
}