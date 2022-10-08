namespace Ekstazz.ProtoGames.Game2.Flow
{
    using System.Threading.Tasks;
    using Cameras;
    using ProtoGames.Cameras;
    using Zenject;
    using Zenject.Extensions.Commands;

    public class ShakeGame2CameraCommand : Command
    {
        [Inject] private LazyInject<Game2CamerasController> gameCamerasController;

        public async override Task Execute()
        {
            gameCamerasController.Value.ShakeCamera(CameraShakeType.Heavy);
        }
    }
}