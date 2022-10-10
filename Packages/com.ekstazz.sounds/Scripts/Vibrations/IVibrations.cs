namespace Ekstazz.Sounds
{
    using MoreMountains.NiceVibrations;

    
    public interface IVibrations
    {
        bool Enabled { get; set; }
        void Impact(HapticTypes type);
        void Impact(float intensity, float sharpness);
        void Impact(float intensity, float sharpness, float duration);
        void Impact(AdvancedVibration vibration);
        void ApplySettings(bool enabled);
    }
}