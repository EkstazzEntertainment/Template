namespace Ekstazz.Sounds
{
    using System;
    using UnityEngine;
    using Random = UnityEngine.Random;

    [Serializable]
    public class Audio
    {
        public float Pitch => Random.Range(minPitch, maxPitch);

        public AudioClip clip;

        [Range(-3.0f, 3.0f)]
        public float minPitch = 1.0f;

        [Range(-3.0f, 3.0f)]
        public float maxPitch = 1.0f;

        [Range(0, 1)]
        public float volume = 1;
    }

    [Serializable]
    public class Music
    {
        public AudioClip clip;

        [Range(0, 1)]
        public float volume;
    }
}