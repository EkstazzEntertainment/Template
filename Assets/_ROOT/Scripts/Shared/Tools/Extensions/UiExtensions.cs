namespace Ekstazz.Tools
{
    using UnityEngine;

    public static class UiExtensions
    {
        public static Vector2 CanvasPoint(this RectTransform canvas, Vector3 screenPoint)
        {
            Vector2 pos;
            var x = screenPoint.x / Screen.width;
            var y = screenPoint.y / Screen.height;

            var canvasRect = canvas.rect;
            pos = new Vector3((x - 0.5f) * canvasRect.width, (y - 0.5f) * canvasRect.height, 0);
            return pos;
        }
        
        public static Vector3 GetPositionWithOffset(Vector3 position, float offset)
        {
            return position + GetScale() * offset * Vector3.up;
        }
        
        private static float GetScale()
        {
            var dpi = GetDPI();
            if (dpi <= 0)
            {
                dpi = 200.0f;
            }
            return 200.0f / dpi;
        }

        public static float GetDPI()
        {
#if UNITY_ANDROID
            var activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            var activity = activityClass.GetStatic<AndroidJavaObject>("currentActivity");

            var metrics = new AndroidJavaObject("android.util.DisplayMetrics");
            activity.Call<AndroidJavaObject>("getWindowManager").Call<AndroidJavaObject>("getDefaultDisplay").Call("getMetrics", metrics);

            return (metrics.Get<float>("xdpi") + metrics.Get<float>("ydpi")) * 0.5f;
#endif
            return Screen.dpi;
        }
    }
}