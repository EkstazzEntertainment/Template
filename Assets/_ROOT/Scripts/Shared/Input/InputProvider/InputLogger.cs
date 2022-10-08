namespace Ekstazz.Input
{
    using Drags;
    using Swipes;
    using UnityEngine;
    using Zenject;

    public class InputLogger : MonoBehaviour
    {
        [Inject]
        public IInputProvider InputProvider { get; set; }

        [Header("Tap")]
        [SerializeField]
        private bool logTap;

        [Header("Hold")]
        [SerializeField]
        private bool logHold;
        
        [Header("Pointer")]
        [SerializeField]
        private bool logPointer;

        [SerializeField]
        private bool logPointerMove;

        [Header("Drag")]
        [SerializeField]
        private bool logDrag;

        [SerializeField]
        private bool logDragMove;

        [Header("Swipe")]
        [SerializeField]
        private bool logSwipe;

        private void Start()
        {
#if !DEBUG
            return;
#endif
            InputProvider.OnTap += OnTap;

            InputProvider.OnHold += OnHold;
            
            InputProvider.OnPointerDown += OnPointerDown;
            InputProvider.OnPointerMove += OnPointerMove;
            InputProvider.OnPointerUp += OnPointerUp;

            InputProvider.OnDragStart += OnDragStart;
            InputProvider.OnDrag += OnDrag;
            InputProvider.OnDragEnd += OnDragEnd;

            InputProvider.OnSwipe += OnSwipe;
        }

        private void OnTap(Vector2 pos)
        {
            if (logTap)
            {
                Debug.Log($"<color=green>Input:</color> Tap at position: {pos}");
            }
        }

        private void OnHold(Vector2 pos)
        {
            if (logHold)
            {
                Debug.Log($"<color=green>Input:</color> Hold at position: {pos}");
            }
        }

        private void OnPointerDown(Vector2 pos)
        {
            if (logPointer)
            {
                Debug.Log($"<color=green>Input:</color> Pointer down at position: {pos}");
            }
        }

        private void OnPointerMove(Vector2 pos)
        {
            if (logPointer && logPointerMove)
            {
                Debug.Log($"<color=green>Input:</color> Pointer move at position: {pos}");
            }
        }

        private void OnPointerUp(Vector2 pos)
        {
            if (logPointer)
            {
                Debug.Log($"<color=green>Input:</color> Pointer up at position: {pos}");
            }
        }

        private void OnDragStart(Drag drag)
        {
            if (logDrag)
            {
                Debug.Log($"<color=green>Input:</color> Drag start event, info: {drag}");
            }
        }

        private void OnDrag(Drag drag)
        {
            if (logDrag && logDragMove)
            {
                Debug.Log($"<color=green>Input:</color> Drag event, info: {drag}");
            }
        }

        private void OnDragEnd(Drag drag)
        {
            if (logDrag)
            {
                Debug.Log($"<color=green>Input:</color> Drag end event, info: {drag}");
            }
        }

        private void OnSwipe(Swipe swipe)
        {
            if (logSwipe)
            {
                Debug.Log($"<color=green>Input:</color> Swipe event, direction: {swipe}");
            }
        }
    }
}
