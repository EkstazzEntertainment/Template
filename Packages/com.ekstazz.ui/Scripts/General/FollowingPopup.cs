namespace Ekstazz.Ui
{
    using DG.Tweening;
    using JetBrains.Annotations;
    using UnityEngine;

    
    public abstract class FollowingPopUp<T> : Window where T : IFolowee
    {
        public T Folowee;

        private Canvas canvas;
        
        public override bool IsBlockingDrag => false;

        
        public void Init(T folowee)
        {
            if (folowee?.Anchor == null)
            {
                this.Close();
            }

            Folowee = folowee;
            canvas = GetComponentInParent<Canvas>();

            UpdatePosition();

            InitInternal();
        }

        protected virtual void InitInternal()
        {
        }

        protected void AnimateShow(float duration, float delay)
        {
            if (Folowee.Anchor == null)
            {
                Close();
                return;
            }

            transform.DOScale(Vector3.zero, duration).From().SetEase(Ease.OutBack).SetDelay(delay);
            UpdatePosition();
        }

        protected void UpdatePosition()
        {
            if (Folowee.Anchor == null)
            {
                return;
            }

            Vector2 p;
            var cam = Camera.main;
            var screenPoint = cam.WorldToScreenPoint(Folowee.Anchor.Value);
            var x = screenPoint.x / Screen.width;
            var y = screenPoint.y / Screen.height;

            var canvasRect = canvas.GetComponent<RectTransform>().rect;
            GetComponent<RectTransform>().localPosition = new Vector3(
                (x - 0.5f) * canvasRect.width, 
                (y - 0.5f) * canvasRect.height, 
                0);
        }

        [UsedImplicitly]
        public void LateUpdate()
        {
            if (Folowee.Anchor != null)
            {
                UpdatePosition();
            }
            else
            {
                Close();
            }
        }
    }

    public interface IFolowee
    {
        Vector3? Anchor { get; }
    }
}
