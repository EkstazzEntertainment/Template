namespace Ekstazz.Sounds
{
    using Zenject;

    
    public class VibroSettingsToggle : SettingsToggle
    {
        [Inject] public IVibrations Vibrations { get; set; }

        protected override bool InitialState => Vibrations.Enabled;
        protected override SettingsType Type => SettingsType.Vibration;

        
        protected override void UpdateSettingsInternal(bool state)
        {
            Vibrations.Enabled = state;
        }
    }
}