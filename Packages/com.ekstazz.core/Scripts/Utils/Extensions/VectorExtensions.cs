namespace Ekstazz.Utils.Extensions
{
    using UnityEngine;

    public static class VectorExtensions
    {
        public static float FromRange(this Vector2 range)
        {
            return range.x + Random.value * (range.y - range.x);
        }

        public static Vector3 Circumcenter(Vector3 a, Vector3 b, Vector3 c)
        {
            var ab = b - a;
            var ac = c - a;

            var abXac = Vector3.Cross(ab, ac);

            var i = ab.sqrMagnitude * ac;
            var j = ac.sqrMagnitude * ab;
            var k = Vector3.Cross(i - j, abXac);

            return a + k * (0.5f / abXac.sqrMagnitude);
        }

        public static Vector3 Round(this Vector3 v, int snapValue = 1)
        {
            return new Vector3(
                snapValue * Mathf.Round(v.x / snapValue),
                snapValue * Mathf.Round(v.y / snapValue),
                snapValue * Mathf.Round(v.z / snapValue));
        }

        public static Vector3 WithZeroHeight(this Vector3 v)
        {
            return new Vector3(v.x, 0, v.z);
        }

        public static Vector3 RoundToIntValues(this Vector3 vector3)
        {
            return new Vector3(Mathf.Round(vector3.x), Mathf.Round(vector3.y), Mathf.Round(vector3.z));
        }
    }
}
