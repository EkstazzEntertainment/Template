namespace Ekstazz.ProtoGames.Game2.Flow
{
    using Cameras;
    using ProtoGames.Flow;
    using UnityEngine;

    public class SwitchToGame2InitialCamera : SwitchToInitialCamera<Game2CamerasController>
    {
        protected override Transform Target => null;
    }
    
    public class SwitchToGame2HomeCamera : SwitchToHomeCamera<Game2CamerasController>
    {
        protected override Transform Target => null;
    }
    
    public class SwitchToGame2GameCamera : SwitchToGameCamera<Game2CamerasController>
    {
        protected override Transform Target => null;
    }
    
    public class SwitchToGame2ZoomCamera : SwitchToZoomCamera<Game2CamerasController>
    {
        protected override Transform Target => null;
    }

    public class SwitchToGame2EndCamera : SwitchToEndCamera<Game2CamerasController>
    {
        protected override Transform Target => null;
    }
}