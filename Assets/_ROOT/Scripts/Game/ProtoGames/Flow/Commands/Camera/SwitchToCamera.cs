namespace Ekstazz.ProtoGames.Flow
{
    using Cameras;
    using UnityEngine;
    using Zenject;
    using Zenject.Extensions.Commands;

    public abstract class SwitchCamera<T> : LockableCommand where T : GameCamerasController<T>
    {
        [Inject] public T GameCamerasController { get; set; }

        protected abstract TypeOfCamera TypeOfCamera { get; }
        protected virtual Transform Target => null;

        
        public override void Execute()
        {
            SetUpCamera();
        }

        protected virtual void SetUpCamera()
        {
            // Lock();
            GameCamerasController.SwitchCameraTo(TypeOfCamera);
            GameCamerasController.CurrentCamera.LookAt(Target);
            GameCamerasController.CurrentCamera.Follow(Target);
            // GameCamerasController.ExecuteAfterBlend(Unlock);
        }
    }

    public class SwitchToInitialCamera<T> : SwitchCamera<T> where T : GameCamerasController<T>
    {
        protected override TypeOfCamera TypeOfCamera => TypeOfCamera.Initial;
    }

    public class SwitchToHomeCamera<T> : SwitchCamera<T> where T : GameCamerasController<T>
    {
        protected override TypeOfCamera TypeOfCamera => TypeOfCamera.Home;
    }

    public class SwitchToGameCamera<T> : SwitchCamera<T> where T : GameCamerasController<T>
    {
        protected override TypeOfCamera TypeOfCamera => TypeOfCamera.Game;
    }
    
    public class SwitchToZoomCamera<T> : SwitchCamera<T> where T : GameCamerasController<T>
    {
        protected override TypeOfCamera TypeOfCamera => TypeOfCamera.Zoom;
    }

    public class SwitchToEndCamera<T> : SwitchCamera<T> where T : GameCamerasController<T>
    {
        protected override TypeOfCamera TypeOfCamera => TypeOfCamera.End;
    }
}