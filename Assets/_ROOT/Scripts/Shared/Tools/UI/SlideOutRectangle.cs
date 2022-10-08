namespace Ekstazz.Shared.Tools.Ui
{
    using System;
    using DG.Tweening;
    using UnityEngine;
    using UnityEngine.Events;

    
    [RequireComponent(typeof(RectTransform))]
    public class SlideOutRectangle : MonoBehaviour
    {
        [Header("Structure")]
        [SerializeField] private RectTransform slidingBody;
        
        [Header("Setup")]
        [SerializeField] private SlidingDirection slidingDirection;
        [SerializeField] private bool isActiveAtStartup;

        [Header("Settings")]
        [SerializeField] private float defaultSlidingDuration = 1f;
        [SerializeField] private float defaultToggleDuration = 2.5f;
        [SerializeField, Range(0, 1)] private float normalizedSlidingPart = 1f;
        [SerializeField] private float additionalSlideValue;
        [SerializeField] private Ease slidingEaseType = Ease.OutQuint;

        [Header("Events")]
        [SerializeField] private UnityEvent onTogglingOn;
        [SerializeField] private UnityEvent onTogglingOff;
        
        private bool isCurrentlyActive;
        private Vector2 transitionVector;
        private Vector2 slidingAnchor;
        
        public bool IsCurrentlyOut => isCurrentlyActive;
        
        
        private void Awake()
        {
            switch (slidingDirection)
            {
                case SlidingDirection.Right:
                    transitionVector = new Vector2(slidingBody.rect.width * slidingBody.localScale.x * normalizedSlidingPart,0);
                    break;
                case SlidingDirection.Left:
                    transitionVector = new Vector2(-slidingBody.rect.width * slidingBody.localScale.x * normalizedSlidingPart,0);
                    break;
                case SlidingDirection.Upside:
                    transitionVector = new Vector2(0,slidingBody.rect.height * slidingBody.localScale.y * normalizedSlidingPart);
                    break;
                case SlidingDirection.Downside:
                    transitionVector = new Vector2(0,-slidingBody.rect.height * slidingBody.localScale.y * normalizedSlidingPart);
                    break;
            }

            slidingAnchor = isActiveAtStartup
                ? slidingBody.anchoredPosition - transitionVector / 2 
                : slidingBody.anchoredPosition + transitionVector/2;

            isCurrentlyActive = isActiveAtStartup;
        }

        public void ToggleSlider()
        {
            ToggleSliderFor(defaultToggleDuration);
        }
        
        public void ToggleSliderFor(float isOnDuration, Action callback = null)
        {
            slidingBody.DOKill();
            if (IsCurrentlyOut)
            {
                DOVirtual.DelayedCall(isOnDuration,() =>
                {
                    SetSliderState(false, () =>
                    {
                        callback?.Invoke();
                    });
                });
            }
            else
            {
                SetSliderState(true, () => 
                { 
                    DOVirtual.DelayedCall(isOnDuration, () =>
                    {
                        SetSliderState(false, () =>
                        {
                            callback?.Invoke();
                        });
                    });
                    
                });
            }
        }
        
        public void SetSliderState(bool isOut) => PerformTransition(isOut,defaultSlidingDuration,null);
        public void SetSliderState(bool isOut, float duration) => PerformTransition(isOut,duration,null);
        public void SetSliderState(bool isOut, Action callback) => PerformTransition(isOut,defaultSlidingDuration,callback);
        public void SetSliderState(bool isOut, float duration, Action callback) => PerformTransition(isOut,duration,callback);

        private void PerformTransition(bool isActive, float transitionDuration, Action callback)
        {
            slidingBody.DOKill();
            var slideValue = isActive ? transitionVector / 2 : -transitionVector / 2;
            var additionalSlide = DirectionNormalizedVector(slidingDirection) * additionalSlideValue;
            additionalSlide *= isActive ? 1 : -1;

            slideValue += additionalSlide;

            if (isActive == false && isCurrentlyActive == true)
            {
                onTogglingOff?.Invoke();
            }
            if (isActive == true && isCurrentlyActive == false)
            {
                onTogglingOn?.Invoke();
            }
            
            isCurrentlyActive = isActive;
            slidingBody.DOAnchorPos(slidingAnchor + slideValue, transitionDuration).SetEase(slidingEaseType).OnComplete(() =>
            {
                callback?.Invoke();
            });
        }

        private void OnDestroy()
        {
            slidingBody.DOKill(true);
            this.DOKill(true);
        }

        private Vector2 DirectionNormalizedVector(SlidingDirection direction)
        {
            switch (direction)
            {
                case SlidingDirection.Right:
                    return Vector2.right;
                case SlidingDirection.Left:
                    return Vector2.left;
                case SlidingDirection.Downside:
                    return Vector2.down;
                case SlidingDirection.Upside:
                    return Vector2.up;
                default:
                    return Vector2.zero;
            }
        }

        private enum SlidingDirection
        {
            Upside,
            Downside,
            Left,
            Right
        }
    }
}