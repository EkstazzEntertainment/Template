namespace Ekstazz.Utils.Extensions
{
    using System;
    using System.Collections;
    using System.Linq;
    using UnityEngine;
    using Object = UnityEngine.Object;

    public static class GameObjectExtensions
    {
        public static bool HasComponent<T>(this GameObject gameObject) where T : MonoBehaviour
        {
            return gameObject.GetComponent<T>() != null;
        }

        public static bool HasComponentInParent<T>(this GameObject gameObject) where T : MonoBehaviour
        {
            return gameObject.GetComponentInParent<T>() != null;
        }

        public static bool HasNotComponent<T>(this GameObject gameObject) where T : MonoBehaviour
        {
            return gameObject.GetComponent<T>() == null;
        }

        public static T GetInactiveComponentInChildren<T>(this MonoBehaviour mono) where T : Component
        {
            return mono.GetComponentsInChildren<T>(true).FirstOrDefault();
        }

        public static T AddOrGetComponent<T>(this GameObject go) where T : Component
        {
            var comp = go.GetComponent<T>();
            return comp ? comp : go.AddComponent<T>();
        }

        public static GameObject AddChild(this GameObject go, string name)
        {
            var child = new GameObject(name);
            child.transform.SetParent(go.transform);
            return child;
        }

        public static void CleanUpChildren(this Transform transform)
        {
            foreach (Transform child in transform)
            {
                Object.Destroy(child.gameObject);
            }
        }

        public static void ExecuteInNextFrame(this MonoBehaviour go, Action action)
        {
            go.StartCoroutine(NextFrameCoroutine(action));
        }

        private static IEnumerator NextFrameCoroutine(Action action)
        {
            yield return null;
            action?.Invoke();
        }

        public static void ExecutePeriodically(this MonoBehaviour go, float delay, Action action)
        {
            go.StartCoroutine(PeriodicalCoroutine(go.gameObject, action, delay));
        }

        private static IEnumerator PeriodicalCoroutine(GameObject go, Action action, float delay)
        {
            while (go)
            {
                action?.Invoke();
                yield return new WaitForSeconds(delay);
            }
        }

        public static void MoveToLayer(this GameObject obj, string layerName)
        {
            var mask = LayerMask.NameToLayer(layerName);
            if (obj.layer == mask)
            {
                return;
            }
            foreach (var child in obj.GetComponentsInChildren<Transform>(true))
            {
                child.gameObject.layer = mask;
            }
        }

        public static RectTransform rectTransform(this Component monoBehaviour)
        {
            return monoBehaviour.GetComponent<RectTransform>();
        }
    }
}
