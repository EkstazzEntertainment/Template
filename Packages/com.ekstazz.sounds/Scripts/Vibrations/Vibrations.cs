namespace Ekstazz.Sounds
{
    using MoreMountains.NiceVibrations;
    using Utils;

    
    public class Vibrations : IVibrations
    {
        private PlayerPrefsStoredValue<bool> enabled;

        public bool Enabled
        {
            get => enabled.Value;
            set => enabled.Value = value;
        }

        public void Impact(HapticTypes type)
        {
            if (Enabled)
            {
                MMVibrationManager.Haptic(type);
            }
        }

        public void Impact(float intensity, float sharpness)
        {
            if (Enabled)
            {
                MMVibrationManager.TransientHaptic(intensity, sharpness);
            }
        }

        public void Impact(float intensity, float sharpness, float duration)
        {
            if (Enabled)
            {
                MMVibrationManager.ContinuousHaptic(intensity, sharpness, duration);
            }
        }

        public void Impact(AdvancedVibration vibration)
        {
            if (Enabled)
            {
                MMVibrationManager.AdvancedHapticPattern(vibration.AHAPFile.text,
                    vibration.waveFormAsset.WaveForm.Pattern,
                    vibration.waveFormAsset.WaveForm.Amplitudes, -1,
                    vibration.rumbleWaveFormAsset.WaveForm.Pattern,
                    vibration.rumbleWaveFormAsset.WaveForm.LowFrequencyAmplitudes,
                    vibration.rumbleWaveFormAsset.WaveForm.HighFrequencyAmplitudes, -1,
                    HapticTypes.LightImpact);
            }
        }

        public void ApplySettings(bool enabled)
        {
            this.enabled = new PlayerPrefsStoredValue<bool>("vibrations", enabled);
        }
    }
}