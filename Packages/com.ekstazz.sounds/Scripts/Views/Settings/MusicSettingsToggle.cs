namespace Ekstazz.Sounds
{
    using Zenject;

    
    public class MusicSettingsToggle : SettingsToggle
    {
        [Inject] public ISounds Sounds { get; set; }

        protected override bool InitialState => Sounds.MusicEnabled;
        protected override SettingsType Type => SettingsType.Music;

        protected override void UpdateSettingsInternal(bool state)
        {
            Sounds.MusicEnabled = state;
        }
    }
}