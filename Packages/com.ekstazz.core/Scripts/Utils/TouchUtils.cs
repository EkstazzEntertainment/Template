namespace Ekstazz.Utils
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public static class TouchUtils
    {
        public static T GetTouchedComponent<T>(Vector3 touchPosition)
        {
            var ray = Camera.main.ScreenPointToRay(touchPosition);
            return Physics.Raycast(ray, out var hit) ? hit.transform.gameObject.GetComponentInParent<T>() : default;
        }

        public static List<T> GetTouchedComponents<T>(Vector3 touchPosition)
        {
            return GetTouchedList(touchPosition)
                .Select(go => go.GetComponent<T>())
                .Where(comp => comp != null)
                .ToList();
        }

        public static List<T> GetTouchedComponentsInParent<T>(Vector3 touchPosition)
        {
            return GetTouchedList(touchPosition)
                .SelectMany(go => go.GetComponentsInParent<T>())
                .Where(comp => comp != null)
                .ToList();
        }

        public static T GetComponentInUi<T>(Vector2 screenPosition) where T : class
        {
            var es = EventSystem.current;
            var list = new List<RaycastResult>();
            es.RaycastAll(new PointerEventData(es) {position = screenPosition}, list);
            var res = list.FirstOrDefault(result => result.gameObject.GetComponent<T>() != null);
            return res.gameObject?.GetComponent<T>();
        }

        public static List<T> GetComponentsInUi<T>(Vector2 screenPosition) where T : class
        {
            var es = EventSystem.current;
            var list = new List<RaycastResult>();
            es.RaycastAll(new PointerEventData(es) {position = screenPosition}, list);
            return list.Select(result => result.gameObject.GetComponent<T>()).Where(obj => obj != null).ToList();
        }

        public static T GetTouchedThroughComponent<T>(Vector3 touchPosition)
        {
            var sortedTouchedList = GetSortedTouchedList(touchPosition);
            var obj = sortedTouchedList.FirstOrDefault(go => go.GetComponentInParent<T>() != null);
            return obj == null ? default : obj.GetComponentInParent<T>();
        }

        public static T GetTouchedComponentInParent<T>(Vector3 touchPosition)
        {
            var ray = Camera.main.ScreenPointToRay(touchPosition);
            return Physics.Raycast(ray, out var hit) ? hit.transform.gameObject.GetComponentInParent<T>() : default;
        }

        public static GameObject GetTouchedObject(Vector3 touchPosition)
        {
            var ray = Camera.main.ScreenPointToRay(touchPosition);
            return Physics.Raycast(ray, out var hit) ? hit.transform.gameObject : null;
        }

        public static List<GameObject> GetSortedTouchedList(Vector3 touchPosition)
        {
            var hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(touchPosition), Vector2.zero);
            return hits.Select(hit => hit.transform.gameObject).ToList();
        }

        public static List<GameObject> GetSortedTouchedListByWorldPoint(Vector3 worldPosition)
        {
            var hits = Physics2D.RaycastAll(worldPosition, Vector2.zero);
            return hits.Select(hit => hit.transform.gameObject).ToList();
        }

        public static List<GameObject> GetTouchedList(Vector3 touchPosition)
        {
            var ray = Camera.main.ScreenPointToRay(touchPosition);
            return Physics.SphereCastAll(ray, 0.3f).Select(hit => hit.transform.gameObject).ToList();
        }

        public static Vector3? HitPoint(Vector3 touchPosition, int layerMask = 0xFFFFFF)
        {
            var ray = Camera.main.ScreenPointToRay(touchPosition);
            if (Physics.Raycast(ray, out var hit, 100000, layerMask))
            {
                return hit.point;
            }
            return null;
        }
    }
}
