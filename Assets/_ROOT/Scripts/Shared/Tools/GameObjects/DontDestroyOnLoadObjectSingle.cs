namespace Ekstazz.Tools
{
    using UnityEngine;

    // It is only a tool. DO NOT USE in actual game logic
    public abstract class DontDestroyOnLoadObjectSingle<T> : MonoBehaviour where T : MonoBehaviour
    {
        private void Awake()
        {
            var instances = FindObjectsOfType<T>();
            if (instances.Length == 1)
            {
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                DestroyImmediate(gameObject);
            }
        }
    }
}
