namespace Ekstazz.Input
{
    using System;
    using Drags;
    using Ekstazz.Utils.Coroutine;
    using Settings;
    using Swipes;
    using UnityEngine;
    using UnityEngine.InputSystem;
    using Zenject;

    
    public class InputProvider : IInputProvider, IInitializable
    {
        [Inject] private CustomInputSettings customInputSettings;
        [Inject] private ICoroutineProvider coroutineProvider;

        public event Action<Vector2> OnTap;

        public event Action<Vector2> OnHold;
        public event Action<float> OnHoldProgress;

        public event Action<Vector2> OnPointerDown;
        public event Action<Vector2> OnPointerMove;
        public event Action<Vector2> OnPointerUp;

        public event Action<Drag> OnDragStart;
        public event Action<Drag> OnDrag;
        public event Action<Drag> OnDragEnd;

        public event Action<Swipe> OnSwipe;

        public Vector2 PointerPosition => Pointer.current.position.ReadValue();

        private InputActions input;
        private TapProcessor tapProcessor;
        private HoldProcessor holdProcessor;
        private DragProcessor dragProcessor;
        private SwipeProcessor swipeProcessor;

        public void Initialize()
        {
            input = new InputActions();
            tapProcessor = new TapProcessor(customInputSettings, input.Gameplay);
            holdProcessor = new HoldProcessor(customInputSettings, coroutineProvider, input.Gameplay);
            dragProcessor = new DragProcessor(customInputSettings, input.Gameplay);
            swipeProcessor = new SwipeProcessor(customInputSettings, dragProcessor);
            BindInputActionsEvents();
            Enable();
        }

        private void BindInputActionsEvents()
        {
            var actions = input.Gameplay;

            tapProcessor.OnTap += position => OnTap?.Invoke(position);

            holdProcessor.OnHold += position => OnHold?.Invoke(position);
            holdProcessor.OnHoldProgress += progress => OnHoldProgress?.Invoke(progress);

            dragProcessor.OnDragStart += drag => OnDragStart?.Invoke(drag);
            dragProcessor.OnDrag += drag => OnDrag?.Invoke(drag);
            dragProcessor.OnDragEnd += drag => OnDragEnd?.Invoke(drag);

            swipeProcessor.OnSwipe += swipe => OnSwipe?.Invoke(swipe);

            actions.Pointer.performed += _ => OnPointerDown?.Invoke(PointerPosition);
            actions.Pointer.canceled += _ => OnPointerUp?.Invoke(PointerPosition);
            actions.PointerMove.performed += context => OnPointerMove?.Invoke(context.ReadValue<Vector2>());
        }

        public void Enable()
        {
            input.Enable();
        }

        public void Disable()
        {
            input.Disable();
        }
    }
}
