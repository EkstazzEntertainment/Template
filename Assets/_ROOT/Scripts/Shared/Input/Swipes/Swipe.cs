namespace Ekstazz.Input.Swipes
{
    using System;

    public class Swipe
    {
        public SwipeDirection Direction { get; }

        public Swipe(SwipeDirection direction)
        {
            Direction = direction;
        }

        public bool CheckDirection(SwipeDirection checkDirection)
        {
            return (Direction & checkDirection) != 0;
        }

        public override string ToString()
        {
            return Direction.ToString();
        }
    }

    [Flags]
    public enum SwipeDirection
    {
        Up = 1 << 1,
        Down = 1 << 2,
        Left = 1 << 3,
        Right = 1 << 4,
        UpLeft = Up | Left,
        UpRight = Up | Right,
        DownLeft = Down | Left,
        DownRight = Down | Right,
    }
}
