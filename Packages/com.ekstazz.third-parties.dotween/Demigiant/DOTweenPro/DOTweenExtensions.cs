namespace Ekstazz.Utils
{
    using DG.Tweening;
    using UnityEngine;

    public static class DOTweenExtensions
    {
        
        public static Tweener FadeIn(this CanvasGroup cg, float duration)
        {
            cg.gameObject.SetActive(true);
            cg.alpha = 0;
            return DOTween.To(() => cg.alpha, value => cg.alpha = value, 1, duration);
        }

        public static Tweener FadeOut(this CanvasGroup cg, float duration)
        {
            var tweener = DOTween.To(() => cg.alpha, value => cg.alpha = value, 0, duration).OnStart(() => cg.alpha = 1);
            tweener.OnComplete(() => cg.gameObject.SetActive(false));
            return tweener;
        }

        public static Tween DoPunch(this GameObject g, float size, float duration)
        {
            return g.transform.DOScale(new Vector3(size, size), duration).SetLoops(2, LoopType.Yoyo);
        }
        
        public static Tweener Punch(this Transform transform, float scale = 1, float size = 0.2f, float duration = 0.5f)
        {
            transform.DOKill();
            transform.localScale = Vector3.one * scale;
            return transform.DOPunchScale(Vector3.one * size, duration, 1);
        }

    }
}