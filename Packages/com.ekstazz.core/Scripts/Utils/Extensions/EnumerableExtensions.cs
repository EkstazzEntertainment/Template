namespace Ekstazz.Utils.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Random = System.Random;

    
    public static class EnumerableExtensions
    {
        public static T RandomElement<T>(this IEnumerable<T> collection)
        {
            return collection.ToList().RandomElement();
        }

        public static T RandomElement<T>(this List<T> list)
        {
            if (!list.Any())
            {
                throw new InvalidOperationException("The source sequence is empty.");
            }

            return list[UnityEngine.Random.Range(0, list.Count)];
        }

        public static T RandomElement<T>(this List<T> list, Random rnd)
        {
            if (!list.Any())
            {
                throw new InvalidOperationException("The source sequence is empty.");
            }
            
            return list[rnd.Next(0, list.Count)];
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> collection)
        {
            var copy = collection.ToList();
            // Knuth shuffle algorithm
            for (var i = 0; i < copy.Count; i++)
            {
                var tmp = copy[i];
                var r = UnityEngine.Random.Range(i, copy.Count);
                copy[i] = copy[r];
                copy[r] = tmp;
            }
            return copy;
        }

        public static string AsString<T>(this IEnumerable<T> list, Func<T, string> converter)
        {
            return string.Join(", ", list.ToList().Select(converter));
        }

        public static T MaxBy<T>(this List<T> list, Converter<T, int> projection)
        {
            if (list.Count == 0)
            {
                throw new InvalidOperationException("Empty list");
            }

            var maxValue = int.MinValue;
            var maxItem = default(T);
            foreach (var item in list)
            {
                var value = projection(item);
                if (value > maxValue)
                {
                    maxValue = value;
                    maxItem = item;
                }
            }
            return maxItem;
        }
    }
}
