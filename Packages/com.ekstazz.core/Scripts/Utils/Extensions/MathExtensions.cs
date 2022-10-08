namespace Ekstazz.Utils.Extensions
{
    using System;
    using UnityEngine;

    public static class MathExtensions
    {
        public static float Randomize(this float value, float amount)
        {
            amount = Mathf.Clamp01(amount);
            return value * (1 - amount / 2) + UnityEngine.Random.value * amount * value;
        }

        public static int CeilTo(this float i, int to)
        {
            return (int) Math.Ceiling(i / to) * to;
        }

        public static bool IsOdd(this int value)
        {
            return value % 2 == 1;
        }
    }
}
