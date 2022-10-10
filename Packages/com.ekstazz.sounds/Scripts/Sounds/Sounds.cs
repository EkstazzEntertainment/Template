namespace Ekstazz.Sounds
{
    using UnityEngine;
    using UnityEngine.Audio;
    using Utils;
    using Zenject;

    
    public class Sounds : ISounds, IInitializable
    {
        private const int MinVolume = -80;

        [Inject] public SoundsView SoundsView { get; set; }

        
        public bool SoundsEnabled
        {
            get => soundsEnabled.Value;
            set => soundsEnabled.Value = value;
        }

        public bool MusicEnabled
        {
            get => musicEnabled.Value;
            set => musicEnabled.Value = value;
        }

        private AudioSource source;
        private AudioMixer mixer;

        private PlayerPrefsStoredValue<bool> musicEnabled;
        private PlayerPrefsStoredValue<bool> soundsEnabled;

        private bool soundsTempValue;
        private bool musicTempValue;

        private float soundsVolume;
        private float musicVolume;

        
        public void Initialize()
        {
            source = SoundsView.Source;
            mixer = SoundsView.Mixer;
        }

        public void PlayTheme(AudioClip clip, float volume = 1f)
        {
            source.clip = clip;
            source.volume = volume;
            source.Play();
        }

        public void PlaySound(AudioClip clip, float volume = 1f, float pitch = 1, float delay = 0, float duration = -1)
        {
            if (clip == null || !SoundsEnabled)
            {
                return;
            }
            SoundsView.Play(clip, volume, delay, pitch, duration);
        }

        public void Mute()
        {
            soundsTempValue = SoundsEnabled;
            SoundsEnabled = false;

            musicTempValue = MusicEnabled;
            MusicEnabled = false;
        }

        public void Unmute()
        {
            SoundsEnabled = soundsTempValue;
            MusicEnabled = musicTempValue;
        }

        public void SetMasterVolume(float musicVolume, float soundsVolume)
        {
            this.musicVolume = (1 - musicVolume) * MinVolume;
            this.soundsVolume = (1 - soundsVolume) * MinVolume;
        }

        public void ApplySettings(bool music, bool sound)
        {
            musicEnabled = new PlayerPrefsStoredValue<bool>(
                "music", 
                value => mixer.SetFloat("music", value ? musicVolume : MinVolume),
                music);
            soundsEnabled = new PlayerPrefsStoredValue<bool>(
                "sound", 
                value => mixer.SetFloat("sound", value ? soundsVolume : MinVolume),
                sound);
        }
    }
}
