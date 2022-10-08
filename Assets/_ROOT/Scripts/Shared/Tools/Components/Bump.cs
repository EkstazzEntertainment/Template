namespace TwoSquares.Helsinki.Shared.Utils
{
    using DG.Tweening;
    using UnityEngine;

    
    public class Bump : MonoBehaviour
    {
        [SerializeField, Range(0.5f, 2f)] protected float scaleValue = 1.1f;
        [SerializeField] protected float scaleTime = 0.7f;
        [SerializeField] private bool manualSwitch;
        [SerializeField] private Ease ease = Ease.Unset;           

        private Tweener tween;

        public bool Enabled
        {
            set
            {
                if (value)
                {
                    Tween();
                }
                else
                {
                    CancelTween();
                }
                
            }
        }
        
        
        private void OnDisable()
        {
            CancelTween();
        }

        private void CancelTween()
        {
            if (tween != null)
            {
                tween.Kill();
                tween.Rewind();
                tween = null;
                transform.localScale = Vector3.one;
            }
        }

        private void OnEnable()
        {
            if (manualSwitch)
            {
                return;
            }
           
            Tween();
        }

        private void Tween()
        {
            CancelTween();
            tween = transform.DOScale(Vector3.one * scaleValue, scaleTime).SetLoops(-1, LoopType.Yoyo).SetEase(ease);
        }
    }
}