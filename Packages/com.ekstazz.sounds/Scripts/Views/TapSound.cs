namespace Ekstazz.Sounds
{
    using UnityEngine;
    using Zenject;

    public abstract class TapSound : MonoBehaviour
    {
        [Inject]
        public GameSounds GameSounds { get; set; }
        
        [SerializeField]
        private Audio overrideSound;
        
        protected void Tap()
        {
            GameSounds.Tap(overrideSound);
        }
    }
}