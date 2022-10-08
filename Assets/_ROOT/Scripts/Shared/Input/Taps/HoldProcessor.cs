namespace Ekstazz.Input
{
    using System;
    using System.Collections;
    using Ekstazz.Utils.Coroutine;
    using Settings;
    using UnityEngine;
    using UnityEngine.InputSystem;
    using UnityEngine.InputSystem.Interactions;

    
    public class HoldProcessor
    {
        public event Action<Vector2> OnHold;
        public event Action<float> OnHoldProgress;

        private static Vector2 PointerPosition => Pointer.current.position.ReadValue();

        private readonly CustomInputSettings settings;
        private readonly ICoroutineProvider coroutineProvider;

        private bool isHolding;
        private Vector2 startPosition;
        private Coroutine coroutine;

        public HoldProcessor(CustomInputSettings settings, ICoroutineProvider coroutineProvider, InputActions.GameplayActions actions)
        {
            this.settings = settings;
            this.coroutineProvider = coroutineProvider;

            actions.Hold.started += context =>
            {
                var slowTap = context.interaction as SlowTapInteraction;
                ProcessPointDown(PointerPosition, slowTap.duration);
            };
            actions.Pointer.canceled += _ => OnPointerUp(PointerPosition);
            actions.PointerMove.performed += _ => OnPointerMove(PointerPosition);
            actions.Hold.performed += _ => ProcessHold(PointerPosition);
        }

        private void ProcessPointDown(Vector2 position, float duration)
        {
            isHolding = true;
            startPosition = position;
            coroutine = coroutineProvider.StartCoroutine(UpdateHoldTime(duration));
        }

        private IEnumerator UpdateHoldTime(float duration)
        {
            var time = 0f;
            while (time < duration)
            {
                OnHoldProgress?.Invoke(time / duration);
                time += Time.deltaTime;
                yield return null;
            }
            OnHoldProgress?.Invoke(1);
        }

        private void OnPointerUp(Vector2 position)
        {
            EndHold();
        }

        private void OnPointerMove(Vector2 position)
        {
            if (!isHolding)
            {
                return;
            }
            if (!IsValidHold(position))
            {
                EndHold();
            }
        }

        private void EndHold()
        {
            isHolding = false;
            if (coroutine != null)
            {
                coroutineProvider.StopCoroutine(coroutine);
            }
            OnHoldProgress?.Invoke(0);
        }

        private void ProcessHold(Vector2 position)
        {
            if (IsValidHold(position))
            {
                OnHold?.Invoke(startPosition);
            }
        }

        private bool IsValidHold(Vector2 position)
        {
            var distance = (position - startPosition).sqrMagnitude;
            return distance <= settings.maxMoveDistance * settings.maxMoveDistance;
        }
    }
}
