namespace Ekstazz.Sounds
{
    using UnityEngine;
    using UnityEngine.UI;
    using Zenject;


    [RequireComponent(typeof(Button))]
    public abstract class TapVibrationButton : MonoBehaviour
    {
        [Inject] public GameSounds GameSounds { get; set; }

        private Button targetButton;

        
        private void Awake()
        {
            targetButton = GetComponent<Button>();
            targetButton.onClick.AddListener(Vibrate);
        }

        public abstract void Vibrate();
    }
}