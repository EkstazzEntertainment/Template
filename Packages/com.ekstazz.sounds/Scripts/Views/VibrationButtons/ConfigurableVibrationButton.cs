namespace Ekstazz.Sounds
{
    using UnityEngine;
    
    
    public class ConfigurableVibrationButton : TapVibrationButton
    {
        [Header("Vibration settings")]
        [SerializeField] private AdvancedVibration vibrationSetup;

        
        public override void Vibrate()
        {
            GameSounds.Haptic(vibrationSetup);
        }
    }
}