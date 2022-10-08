namespace Ekstazz.Input
{
    using System;
    using Settings;
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class TapProcessor
    {
        public event Action<Vector2> OnTap;

        private static Vector2 PointerPosition => Pointer.current.position.ReadValue();

        private readonly CustomInputSettings settings;

        private Vector2 startPosition;

        public TapProcessor(CustomInputSettings settings, InputActions.GameplayActions actions)
        {
            this.settings = settings;

            actions.Tap.started += _ => ProcessPointDown(PointerPosition);
            actions.Tap.performed += _ => ProcessTap(PointerPosition);
        }

        private void ProcessPointDown(Vector2 position)
        {
            startPosition = position;
        }

        private void ProcessTap(Vector2 position)
        {
            var distance = (position - startPosition).sqrMagnitude;
            if (distance <= settings.maxMoveDistance * settings.maxMoveDistance)
            {
                OnTap?.Invoke(startPosition);
            }
        }
    }
}
