namespace Ekstazz.Game.Flow
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class InitialSplash : MonoBehaviour
    {
        private IEnumerator Start()
        {
            yield return null;
            
            yield return SceneManager.LoadSceneAsync(Scenes.Loader, LoadSceneMode.Additive);

            SceneManager.UnloadSceneAsync(Scenes.Splash);
        }
    }
}