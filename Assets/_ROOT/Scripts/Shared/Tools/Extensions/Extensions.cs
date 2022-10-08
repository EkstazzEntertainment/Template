namespace Ekstazz.Tools
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    
    public static class Extensions
    {
        public static List<T> Shuffle<T>(this IEnumerable<T> list)
        {
            var copy = list.ToList();
            for (var i = 0; i < copy.Count; i++)
            {
                var tmp = copy[i];
                var r = Random.Range(i, copy.Count);
                copy[i] = copy[r];
                copy[r] = tmp;
            }
            return copy;
        }
        
        public static Vector3 ProjectOnContactPlane(Vector3 direction, Vector3 normal)
        {
            return direction - normal * Vector3.Dot(direction, normal);
        }
    }
}