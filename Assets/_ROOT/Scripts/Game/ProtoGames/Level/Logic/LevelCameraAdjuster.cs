namespace Ekstazz.ProtoGames.Level.Logic
{
    using Cameras;
    using Ekstazz.LevelBased.Logic;
    using LevelBased.Flow.Signals;
    using LevelBased.Logic;
    using World;
    using Zenject;

    public class LevelCameraAdjuster<TX, TY, TZ> : IInitializable
        where TX : GameCamerasController<TX>
        where TY : ILevelReadyToStart
        where TZ : ILevelCompleted
    {
        [Inject] private ILevelProvider levelProvider;
        [Inject] private IWorldThemeProvider worldThemeProvider;
        [Inject] private SignalBus signalBus;
        [Inject] private LazyInject<TX> camerasController;


        public void Initialize()
        {
            signalBus.Subscribe<TZ>(TurnOffCameras);
            signalBus.Subscribe<TY>(SetUpCurrentLevelCamera);
        }

        private void TurnOffCameras()
        {
            camerasController.Value.TurnOffCameras();
        }

        private void SetUpCurrentLevelCamera(TY signal)
        {
            camerasController.Value.ResetCameras();

            var worldTheme = worldThemeProvider.GetWorldTheme(levelProvider.CurrentLevelGameInfo);

            foreach (var camInfo in worldTheme.cameraOverrideInfos)
            {
                var cam = camerasController.Value.GetCamera(camInfo.cameraType);
                cam.SetFollowOffset(camInfo.camPos);
                cam.SetTrackedObjectOffset(camInfo.camRot);
                cam.Camera.m_Lens.FieldOfView = camInfo.fieldOfView;
            }
        }
    }
}