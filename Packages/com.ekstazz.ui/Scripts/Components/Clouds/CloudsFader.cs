namespace Ekstazz.Ui.Faders.Clouds
{
    using System;
    using DG.Tweening;
    using Faders;
    using UnityEngine;

    
    public class CloudsFader : UiFader
    {
        [SerializeField, Range(0.3f, 3)] private float duration = 1;

        private CloudsTransition clouds;

        
        private void Awake()
        {
            clouds = FindObjectOfType<CloudsTransition>();
        }

        public override void FadeIn(Action onFinished = null)
        {
            if (!clouds)
            {
                onFinished?.Invoke();
                return;
            }

            gameObject.SetActive(false);
            clouds.Show();
            DOVirtual.DelayedCall(duration, () =>
            {
                gameObject.SetActive(true);
                onFinished?.Invoke();
                clouds.Hide();
            });
        }

        public override void FadeOut(Action onFinished = null)
        {
            if (!clouds)
            {
                onFinished?.Invoke();
                return;
            }

            clouds.Show();
            DOVirtual.DelayedCall(duration, () =>
            {
                onFinished?.Invoke();
                clouds.Hide();
            });
        }
    }
}
