namespace Ekstazz.Sounds
{
    using UnityEngine;

    
    public interface ISounds
    {
        bool SoundsEnabled { get; set; }
        bool MusicEnabled { get; set; }
        void PlayTheme(AudioClip clip, float volume = 1f);
        void PlaySound(AudioClip clip, float volume = 1f, float pitch = 1, float delay = 0, float duration = -1);
        void Mute();
        void Unmute();
        void ApplySettings(bool music, bool sound);
        void SetMasterVolume(float musicVolume, float soundsVolume);
    }
}
