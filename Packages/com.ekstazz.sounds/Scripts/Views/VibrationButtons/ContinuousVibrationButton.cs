namespace Ekstazz.Sounds
{
    using UnityEngine;

    public class ContinuousVibrationButton : TapVibrationButton
    {
        [Header("Vibration settings")]
        [SerializeField]
        private ContinuousVibration vibrationSetup;

        public override void Vibrate()
        {
            GameSounds.Haptic(vibrationSetup);
        }
    }
}