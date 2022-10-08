namespace Ekstazz.Game.Flow
{
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class GameSceneChecker : MonoBehaviour
    {
        private void Start()
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(Scenes.Game));
        }
    }
}