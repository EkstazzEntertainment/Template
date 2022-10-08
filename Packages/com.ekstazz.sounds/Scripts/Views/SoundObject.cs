namespace Ekstazz.Sounds
{
    using UnityEngine;

    [RequireComponent(typeof(AudioSource))]
    public class SoundObject : MonoBehaviour
    {
        private AudioSource source;
        private float time;
        
        public void Play(AudioClip clip, float volume, float delay, float pitch, float duration)
        {
            source = GetComponent<AudioSource>();
            time = duration < 0 || duration > clip.length ? clip.length : duration;
            source.clip = clip;
            source.volume = volume;
            source.pitch = pitch;
            source.PlayDelayed(delay);
        }
        
        private void Update()
        {
            if (!source.isPlaying || time <= 0)
            {
                Destroy(gameObject);
            }
            time -= Time.deltaTime;
        }
    }
}