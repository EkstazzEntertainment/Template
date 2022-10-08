namespace Ekstazz.Input.Drags
{
    using System;
    using Settings;
    using UnityEngine;
    using UnityEngine.InputSystem;

    
    public class DragProcessor
    {
        public event Action<Drag> OnDragStart;
        public event Action<Drag> OnDrag;
        public event Action<Drag> OnDragEnd;

        public Vector2 PointerPosition => Pointer.current.position.ReadValue();

        private readonly CustomInputSettings settings;

        private Drag drag;
        private Vector2 startPosition;
        private bool isHolding;
        private bool isDragStarted;

        
        public DragProcessor(CustomInputSettings settings, InputActions.GameplayActions actions)
        {
            this.settings = settings;

            actions.Pointer.performed += _ => ProcessPointDown(PointerPosition);
            actions.Pointer.canceled += _ => ProcessEndDrag(PointerPosition);
            actions.PointerMove.performed += context => ProcessMove(context.ReadValue<Vector2>());
        }

        private void ProcessPointDown(Vector2 position)
        {
            isHolding = true;
            startPosition = position;
        }

        private void ProcessMove(Vector2 position)
        {
            if (!isHolding)
            {
                return;
            }
            if (CheckDragThreshold(position))
            {
                isDragStarted = true;
                StartDrag(position);
            }
            else if (isDragStarted)
            {
                ProcessDrag(position);
            }
        }

        private bool CheckDragThreshold(Vector2 position)
        {
            if (isDragStarted)
            {
                return false;
            }
            var delta = position - startPosition;
            return delta.sqrMagnitude >= settings.dragThreshold * settings.dragThreshold;
        }

        private void StartDrag(Vector2 position)
        {
            drag = Drag.CreateInitialDrag(startPosition, position);
            OnDragStart?.Invoke(drag);
        }

        private void ProcessDrag(Vector2 position)
        {
            drag = Drag.UpdateDrag(drag, position);
            OnDrag?.Invoke(drag);
        }

        private void ProcessEndDrag(Vector2 position)
        {
            if (isDragStarted)
            {
                EndDrag(position);
            }
            isHolding = false;
            isDragStarted = false;
        }

        private void EndDrag(Vector2 position)
        {
            drag = Drag.UpdateDrag(drag, position);
            OnDragEnd?.Invoke(drag);
            drag = null;
        }
    }
}
