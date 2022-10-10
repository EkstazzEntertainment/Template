namespace Ekstazz.Sounds
{
    using Zenject;

    
    public class SoundSettingsToggle : SettingsToggle
    {
        [Inject] public ISounds Sounds { get; set; }

        protected override bool InitialState => Sounds.SoundsEnabled;
        protected override SettingsType Type => SettingsType.Sound;

        protected override void UpdateSettingsInternal(bool state)
        {
            Sounds.SoundsEnabled = state;
        }
    }
}