namespace Ekstazz.Sounds
{
    using UnityEngine;
    using UnityEngine.UI;
    using Zenject;

    [RequireComponent(typeof(Toggle))]
    public abstract class SettingsToggle : MonoBehaviour
    {
        [Inject]
        public SignalBus SignalBus { get; set; }

        [SerializeField]
        private bool inverted = true;

        protected abstract bool InitialState { get; }

        protected abstract SettingsType Type { get; }

        private Toggle toggle;

        private void Awake()
        {
            toggle = GetComponent<Toggle>();
        }

        private void Start()
        {
            toggle.isOn = inverted ? !InitialState : InitialState;
            toggle.onValueChanged.AddListener(value => UpdateSettings(inverted ? !value : value));
        }

        private void UpdateSettings(bool state)
        {
            UpdateSettingsInternal(state);
            SignalBus.Fire(new SettingsChanged() {type = Type, state = state});
        }

        protected abstract void UpdateSettingsInternal(bool state);
    }
}