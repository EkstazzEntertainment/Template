namespace Ekstazz.Utils.Extensions
{
    using Utils;
    using UnityEngine;

    public static class CameraExtensions
    {
        public static Vector2 ToScreenPoint(this Vector3 worldPoint)
        {
            return RectTransformUtility.WorldToScreenPoint(Camera.main, worldPoint);
        }

        public static Vector2 ScreenPointToLocalPoint(this RectTransform rt, Vector2 screenPoint)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, screenPoint, Camera.main, out Vector2 result);
            return result;
        }

        public static Vector2 CanvasPoint(this GameObject tr, Vector3 screenPoint)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(tr.GetComponent<RectTransform>(), screenPoint,
                Camera.main, out var pos);
            return pos;
        }

        public static Vector3 GetScreenPosition(this Transform transform, Canvas canvas, Camera cam)
        {
            var width = canvas.GetComponent<RectTransform>().sizeDelta.x;
            var height = canvas.GetComponent<RectTransform>().sizeDelta.y;
            var x = cam.WorldToScreenPoint(transform.position).x / Screen.width;
            var y = cam.WorldToScreenPoint(transform.position).y / Screen.height;
            var pos = new Vector3(width * x - width / 2, y * height - height / 2);
            return pos;
        }

        public static Vector3? ScreenPointToRectPoint(this Vector2 position, RectTransform rect)
        {
            var hit = TouchUtils.HitPoint(position);
            if (hit.HasValue)
            {
                var screenPoint = Camera.main.WorldToScreenPoint(hit.Value);
                RectTransformUtility.ScreenPointToWorldPointInRectangle(rect, screenPoint, Camera.main, out var p);
                return p;
            }
            return null;
        }

        public static Vector3 ZoomToCamera(this Vector3 point, float length)
        {
            var vector = Camera.main.transform.position - point;
            vector.Normalize();
            return point + vector * length;
        }
    }
}
