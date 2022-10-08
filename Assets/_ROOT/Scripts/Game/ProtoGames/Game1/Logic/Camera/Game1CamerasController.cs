namespace Ekstazz.ProtoGames.Game1.Cameras
{
    using ProtoGames.Cameras;

    public class Game1CamerasController : GameCamerasController<Game1CamerasController>
    {
        protected override string SettingsPath { get; set; } = "Game1";
    }
}