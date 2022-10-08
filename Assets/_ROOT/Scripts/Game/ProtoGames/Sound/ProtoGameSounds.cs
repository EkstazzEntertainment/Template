namespace Ekstazz.ProtoGames.Sound
{
    using Ekstazz.Sounds;

    public class ProtoGameSounds : GameSounds
    {
        private ProtoSoundSettings soundData;

        public override void Initialize()
        {
            base.Initialize();
            soundData = (ProtoSoundSettings)SoundSettings;
        }
    }
}
