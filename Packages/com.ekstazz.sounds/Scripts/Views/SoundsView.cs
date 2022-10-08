namespace Ekstazz.Sounds
{
    using UnityEngine;
    using UnityEngine.Audio;

    [RequireComponent(typeof(AudioSource))]
    public class SoundsView : MonoBehaviour
    {
        [SerializeField]
        private SoundObject soundPrefab;

        [SerializeField]
        private AudioMixer mixer;

        [SerializeField]
        private AudioSource audioSource;

        public AudioMixer Mixer => mixer;

        public AudioSource Source => audioSource;

        public void Play(AudioClip clip, float volume, float delay, float pitch, float duration)
        {
            var sound = Instantiate(soundPrefab, transform);
            sound.Play(clip, volume, delay, pitch, duration);
        }
    }
}