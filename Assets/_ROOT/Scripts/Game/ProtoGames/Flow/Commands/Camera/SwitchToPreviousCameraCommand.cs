namespace Ekstazz.ProtoGames.Flow
{
    using Cameras;
    using Ekstazz.LevelBased.Logic.State;
    using UnityEngine;
    using Zenject;


    public class SwitchToPreviousCameraCommand<T> : SwitchCamera<T> where T : GameCamerasController<T>
    {
        [Inject] private ILevelStateProvider levelStateProvider;

        protected override TypeOfCamera TypeOfCamera => GetCameraType();
        protected override Transform Target => null;

        
        private TypeOfCamera GetCameraType()
        {
            return levelStateProvider.CurrentState == LevelState.Completing ? TypeOfCamera.End : TypeOfCamera.Game;
        }
    }
}
