namespace Ekstazz.Sounds
{
    public class SettingsChanged
    {
        public SettingsType type;

        public bool state;
    }

    public enum SettingsType
    {
        Sound,
        Music,
        Vibration
    }
}