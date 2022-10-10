namespace Ekstazz.Ui.Faders
{
    using System;
    using Utils;
    using DG.Tweening;
    using JetBrains.Annotations;
    using UnityEngine;
    using Utils.Extensions;

    
    public class BumpCrossFader : UiFader
    {
        [SerializeField, Range(0.1f, 1)] private float scale = 0.8f;
        [SerializeField, Range(0.1f, 1f)] private float time = 0.2f;

        private CanvasGroup cg;

        
        [UsedImplicitly]
        public void Awake()
        {
            cg = gameObject.AddOrGetComponent<CanvasGroup>();
        }

        public override void FadeIn(Action onFinished = null)
        {
            transform.DOScale(scale, time).From().SetEase(Ease.OutBack).OnComplete(() => onFinished?.Invoke());
            cg.FadeIn(0.1f);
        }

        public override void FadeOut(Action onFinished = null)
        {
            if (!this)
            {
                return;
            }
            transform.DOScale(0.9f, 0.15f);
            cg.FadeOut(0.15f).OnComplete(() => onFinished?.Invoke());
        }
    }
}
