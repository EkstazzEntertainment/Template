namespace Ekstazz.Tools
{
    using System;
    using System.Collections.Generic;

    
    public class WeightedMap<T> : IProbabilityMap<T>
    {
        private readonly Random random;

        private float ceil;
        private List<Tuple<T, float>> tuples = new();
        private Tuple<float, float>[] weights;

        public WeightedMap(Random random)
        {
            this.random = random;
        }

        
        public void Add(T t, float weight)
        {
            tuples.Add(new Tuple<T, float>(t, weight));
            Recalculate();
        }

        public void Add(T t)
        {
            Add(t, 1);
        }

        public void Remove(T t)
        {
            var toRemove = -1;
            for (int i = 0, tuplesSize = tuples.Count; i < tuplesSize; i++)
            {
                if (tuples[i].Item1.Equals(t))
                {
                    toRemove = i;
                }
            }
            if (toRemove != -1)
            {
                tuples.RemoveAt(toRemove);
                Recalculate();
            }
        }

        public void Clear()
        {
            tuples = new List<Tuple<T, float>>();
            Recalculate();
        }

        public IProbabilityMap<T> Where(Predicate<T> predicate)
        {
            var result = new WeightedMap<T>(random);
            foreach (var (item, weight) in tuples)
            {
                if (predicate(item))
                {
                    result.Add(item, weight);
                }
            }

            return result;
        }

        public T Get()
        {
            var r = random.NextDouble() * ceil;
            for (int i = 0, weightsLength = weights.Length; i < weightsLength; i++)
            {
                var weight = weights[i];
                if (weight.Item1 < r && r <= weight.Item2)
                {
                    return tuples[i].Item1;
                }
            }
            return default;
        }

        private void Recalculate()
        {
            weights = new Tuple<float, float>[tuples.Count];
            var previous = 0f;
            for (int i = 0, tuplesSize = tuples.Count; i < tuplesSize; i++)
            {
                var tuple = tuples[i];
                ceil = previous + tuple.Item2;
                weights[i] = new Tuple<float, float>(previous, ceil);
                previous = ceil;
            }
        }

        public List<Tuple<T, float>> GetTuples()
        {
            return tuples;
        }

        public override string ToString()
        {
            return "WeightedMap :" + "tuples=" + tuples;
        }
    }
}