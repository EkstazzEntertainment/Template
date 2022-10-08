namespace Ekstazz.ProtoGames.Level
{
    using UnityEngine;

    public class SceneLocationMainParent : MonoBehaviour
    {
        [SerializeField] private Light dirLight;

        private void Awake()
        {
            gameObject.SetActive(false);
            dirLight.gameObject.SetActive(false);    
        }
        
        private void OnValidate()
        {
            gameObject.name = nameof(SceneLocationMainParent);
        }
    }
}