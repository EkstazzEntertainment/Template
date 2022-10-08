namespace Ekstazz.ProtoGames.Game2.Cameras
{
    using ProtoGames.Cameras;

    public class Game2CamerasController : GameCamerasController<Game2CamerasController>
    {
        protected override string SettingsPath { get; set; } = "Game2";
    }
}