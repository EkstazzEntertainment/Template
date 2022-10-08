namespace Ekstazz.ProtoGames.Game1.Flow
{
    using System.Threading.Tasks;
    using Cameras;
    using ProtoGames.Cameras;
    using Zenject;
    using Zenject.Extensions.Commands;

    public class ShakeGame1CameraCommand : Command
    {
        [Inject] private LazyInject<Game1CamerasController> gameCamerasController;

        public async override Task Execute()
        {
            gameCamerasController.Value.ShakeCamera(CameraShakeType.Heavy);
        }
    }
}