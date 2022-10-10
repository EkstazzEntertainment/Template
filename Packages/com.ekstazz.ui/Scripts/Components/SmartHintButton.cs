namespace Ekstazz.MyGym.View.UI.HUD.Elements.Buttons
{
    using Ekstazz.Utils;
    using DG.Tweening;
    using UnityEngine;
    
    
    public abstract class SmartHintButton : MonoBehaviour
    {
        [SerializeField] private GameObject hintObject;

        private Tweener moveTween;
        
        public bool IsActive { get; private set; }

        public abstract void OnClick();

        
        private void Start()
        {
            Subscribe();
        }

        private void OnDestroy()
        {
            Unsubscribe();
        }

        protected void TryShow()
        {
            if (IsActive)
            {
                InitMove();
            }
            else
            {
                //in case of selection switching, it have to deselect\select in one frame
                if (DOTween.IsTweening(hintObject.transform))
                {
                    hintObject.transform.DOKill();
                    hintObject.transform.localScale = Vector3.one;
                    hintObject.transform.Punch();
                }
                else
                {
                    hintObject.transform.DOScale(Vector3.zero, 0.15f).From().SetEase(Ease.OutBack);
                }

                hintObject.SetActive(true);
                InitMove();
            }

            IsActive = true;
            Init();

            void InitMove()
            {
                moveTween?.Rewind();
            }
        }

        protected abstract void Subscribe();
        protected abstract void Init();
        protected abstract void Unsubscribe();

        protected void TryHide()
        {
            var wasActive = IsActive;
            IsActive = false;
            if (wasActive)
            {
                hintObject.transform.DOScale(Vector3.zero, 0.12f)
                    .OnComplete(() =>
                    {
                        hintObject.transform.localScale = Vector3.one;
                        hintObject.SetActive(false);
                    });

                moveTween?.Rewind();
            }
        }
    }
}
