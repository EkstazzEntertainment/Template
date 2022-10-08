namespace Ekstazz.Input.Settings
{
    using UnityEngine;

    [CreateAssetMenu(fileName = nameof(CustomInputSettings), menuName = "Input/Settings")]
    public class CustomInputSettings : ScriptableObject
    {
        [Header("Tap")]
        public float maxMoveDistance;

        [Header("Drag")]
        public float dragThreshold;

        [Header("Swipe")]
        public float swipeThreshold;

        public float maxSwipeTime;

        [Range(0, 90)]
        public float swipeAngle;
    }
}
