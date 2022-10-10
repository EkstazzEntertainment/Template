namespace Ekstazz.Sounds
{
    using MoreMountains.NiceVibrations;
    using UnityEngine;
    using Zenject;

    
    public abstract class GameSounds : IInitializable
    {
        [Inject] public ISounds Sounds { get; set; }
        [Inject] public IVibrations Vibrations { get; set; }
        [Inject] public SoundSettings SoundSettings { get; set; }

        
        public virtual void Initialize()
        {
            var musicVolume = SoundSettings.masterMusicVolume;
            var soundVolume = SoundSettings.masterSoundsVolume;
            Sounds.SetMasterVolume(musicVolume, soundVolume);
            var music = SoundSettings.MusicEnabled;
            var sound = SoundSettings.SoundsEnabled;
            Sounds.ApplySettings(music, sound);
            var vibro = SoundSettings.VibrationsEnabled;
            Vibrations.ApplySettings(vibro);
        }

        public void Tap(Audio overrideSound)
        {
            Play(overrideSound ?? SoundSettings.tap);
            Haptic(SoundSettings.tapVibro);
        }

        public void Play(AudioClip clip)
        {
            Sounds.PlaySound(clip);
        }

        public void Play(Audio sound)
        {
            Sounds.PlaySound(sound.clip, sound.volume, sound.Pitch);
        }

        public void Play(Music music)
        {
            Sounds.PlayTheme(music.clip, music.volume);
        }

        public void Haptic(HapticTypes type)
        {
            Vibrations.Impact(type);
        }

        public void Haptic(Vibration vibration)
        {
            Vibrations.Impact(vibration.intensity, vibration.sharpness);
        }

        public void Haptic(ContinuousVibration vibration)
        {
            Vibrations.Impact(vibration.intensity, vibration.sharpness, vibration.duration);
        }

        public void Haptic(AdvancedVibration vibration)
        {
            Vibrations.Impact(vibration);
        }
    }
}
