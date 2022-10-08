namespace Ekstazz.Input.Drags
{
    using UnityEngine;

    
    public class Drag
    {
        public Vector2 StartPosition { get; }
        public Vector2 PreviousPosition { get; }
        public Vector2 CurrentPosition { get; }
        public Vector2 Delta { get; }

        public Vector2 OverallDelta => CurrentPosition - StartPosition;

        
        public Drag(Vector2 currentPosition, Vector2 previousPosition, Vector2 startPosition)
        {
            CurrentPosition = currentPosition;
            PreviousPosition = previousPosition;
            StartPosition = startPosition;

            Delta = CurrentPosition - PreviousPosition;
        }

        public float GetOverallRotation(Vector2 pointPosition)
        {
            return Vector2.SignedAngle(StartPosition - pointPosition, CurrentPosition - pointPosition);
        }

        public float GetDeltaRotation(Vector2 pointPosition)
        {
            return Vector2.SignedAngle(PreviousPosition - pointPosition, CurrentPosition - pointPosition);
        }
        
        public static Drag CreateInitialDrag(Vector2 startPosition, Vector2 currentPosition)
        {
            return new Drag(currentPosition, startPosition, startPosition);
        }

        public static Drag UpdateDrag(Drag drag, Vector2 сurrentPosition)
        {
            return new Drag(сurrentPosition, drag.CurrentPosition, drag.StartPosition);
        }
        
        public override string ToString()
        {
            return $"(DragInfo) start position: {StartPosition}, " +
                   $"previous position: {PreviousPosition}, " +
                   $"current position: {CurrentPosition}, " +
                   $"delta: {Delta}, " +
                   $"overall delta: {OverallDelta}";
        }
    }
}