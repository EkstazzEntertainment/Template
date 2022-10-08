namespace Ekstazz.Input
{
    using System;
    using Drags;
    using Swipes;
    using UnityEngine;

    public interface IInputProvider
    {
        event Action<Vector2> OnTap;

        event Action<Vector2> OnHold;
        event Action<float> OnHoldProgress;

        event Action<Vector2> OnPointerDown;
        event Action<Vector2> OnPointerMove;
        event Action<Vector2> OnPointerUp;

        event Action<Drag> OnDragStart;
        event Action<Drag> OnDrag;
        event Action<Drag> OnDragEnd;

        event Action<Swipe> OnSwipe;

        Vector2 PointerPosition { get; }

        void Enable();

        void Disable();
    }
}
