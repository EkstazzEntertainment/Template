namespace Ekstazz.Input.Swipes
{
    using System;
    using Drags;
    using Settings;
    using UnityEngine;

    public class SwipeProcessor
    {
        public event Action<Swipe> OnSwipe;

        private readonly CustomInputSettings settings;
        private readonly DragProcessor dragProcessor;
        private readonly float swipeAngle;
        private float startSwipeTime;

        
        public SwipeProcessor(CustomInputSettings settings, DragProcessor dragProcessor)
        {
            this.settings = settings;
            this.dragProcessor = dragProcessor;
            swipeAngle = Mathf.Cos(settings.swipeAngle * Mathf.Deg2Rad);

            dragProcessor.OnDragStart += OnDragStart;
            dragProcessor.OnDragEnd += OnDragEnd;
        }

        private void OnDragStart(Drag drag)
        {
            startSwipeTime = Time.time;
        }

        private void OnDragEnd(Drag drag)
        {
            if (ShouldProcessSwipe(drag))
            {
                ProcessSwipe(drag);
            }
        }

        private bool ShouldProcessSwipe(Drag drag)
        {
            var delta = drag.OverallDelta.sqrMagnitude;
            var time = Time.time - startSwipeTime;
            return time <= settings.maxSwipeTime && delta >= settings.swipeThreshold * settings.swipeThreshold;
        }

        private void ProcessSwipe(Drag drag)
        {
            var direction = drag.OverallDelta.normalized;
            var horizontalDirection = GetHorizontalDirection(direction);
            var verticalDirection = GetVerticalDirection(direction);
            var swipeDirection = horizontalDirection | verticalDirection;
            if (swipeDirection != 0)
            {
                FireSwipeEvent(swipeDirection);
            }
        }

        private SwipeDirection GetHorizontalDirection(Vector2 direction)
        {
            var cos = direction.x;
            if (cos >= swipeAngle)
            {
                return SwipeDirection.Right;
            }
            if (cos <= -swipeAngle)
            {
                return SwipeDirection.Left;
            }
            return 0;
        }

        private SwipeDirection GetVerticalDirection(Vector2 direction)
        {
            var sin = direction.y;
            if (sin >= swipeAngle)
            {
                return SwipeDirection.Up;
            }
            if (sin <= -swipeAngle)
            {
                return SwipeDirection.Down;
            }
            return 0;
        }

        private void FireSwipeEvent(SwipeDirection swipeDirection)
        {
            var swipe = new Swipe(swipeDirection);
            OnSwipe?.Invoke(swipe);
        }
    }
}
