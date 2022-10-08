namespace Ekstazz.Ui.Faders
{
    using System;
    using DG.Tweening;
    using JetBrains.Annotations;
    using UnityEngine;

    public class RightLeftFader : UiFader
    {
        [SerializeField]
        private bool invert;

        [SerializeField]
        private float duration = 0.2f;

        [SerializeField]
        private int delta;

        [UsedImplicitly]
        public void Awake()
        {
            delta = invert ? delta : -delta;
        }

        public override void FadeOut(Action onFinished = null)
        {
            var pos = transform.position;

            transform.DOLocalMoveX(delta, duration).SetRelative(true).OnComplete(() =>
            {
                transform.position = pos;
                onFinished?.Invoke();
            });
        }

        public override void FadeIn(Action onFinished = null)
        {
            transform.DOLocalMoveX(delta, duration).From(true).OnComplete(() => { onFinished?.Invoke(); });
        }
    }
}
