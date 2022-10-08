namespace Ekstazz.Tools
{
    using Ekstazz.Core;
    using JetBrains.Annotations;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using Zenject;

    public class StartSceneLoader : MonoBehaviour
    {
        [SerializeField] private string defaultSceneToLoad = "Splash";
        [SerializeField] private SceneContext sceneContext;

        
        [UsedImplicitly]
        private void Awake()
        {
#if UNITY_EDITOR
            var appRoot = FindObjectOfType<AppRoot>();
            if (appRoot == null)
            {
                Debug.LogWarning($"AppRoot has not initialized yet, loading {defaultSceneToLoad}");
                SceneManager.LoadScene(defaultSceneToLoad);
                return;
            }
#endif
            sceneContext.Run();
        }
    }
}