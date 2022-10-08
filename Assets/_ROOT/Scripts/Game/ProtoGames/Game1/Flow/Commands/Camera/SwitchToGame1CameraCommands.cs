namespace Ekstazz.ProtoGames.Game1.Flow
{
    using Cameras;
    using ProtoGames.Flow;
    using UnityEngine;

    public class SwitchToGame1InitialCamera : SwitchToInitialCamera<Game1CamerasController>
    {
        protected override Transform Target => null;
    }
    
    public class SwitchToGame1HomeCamera : SwitchToHomeCamera<Game1CamerasController>
    {
        protected override Transform Target => null;
    }
    
    public class SwitchToGame1GameCamera : SwitchToGameCamera<Game1CamerasController>
    {
        protected override Transform Target => null;
    }
    
    public class SwitchToGame1ZoomCamera : SwitchToZoomCamera<Game1CamerasController>
    {
        protected override Transform Target => null;
    }
    
    public class SwitchToGame1EndCamera : SwitchToEndCamera<Game1CamerasController>
    {
        protected override Transform Target => null;
    }
}