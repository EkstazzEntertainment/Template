namespace Ekstazz.Tools
{
    using System.Collections.Generic;

    
    public class RandomPicker<T>
    {
        private WeightedMap<T> map;

        private readonly List<T> excluded = new List<T>();

        public RandomPicker(WeightedMap<T> map)
        {
            this.map = map;
        }

        
        public void Exclude(T t)
        {
            excluded.Add(t);
        }

        public void Reset()
        {
            excluded.Clear();
        }

        public T Get()
        {
            T element;
            do
            {
                element = map.Get();
            } while (excluded.Contains(element));

            return element;
        }
    }
}