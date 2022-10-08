namespace Ekstazz.Tools
{
    using UnityEngine;

    public class DisableOnPlayMode : MonoBehaviour
    {
        private void Awake()
        {
            gameObject.SetActive(false);
        }
    }
}