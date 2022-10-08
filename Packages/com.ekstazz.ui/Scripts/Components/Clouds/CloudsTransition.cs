namespace Ekstazz.Ui.Faders.Clouds
{
    using UnityEngine;

    public class CloudsTransition : MonoBehaviour
    {
        private Animator animator;

        private string key = "CloudTransitionFX";

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void Show()
        {
            animator.SetBool(key, true);
        }

        public void Hide()
        {
            animator.SetBool(key, false);
        }
    }
}
