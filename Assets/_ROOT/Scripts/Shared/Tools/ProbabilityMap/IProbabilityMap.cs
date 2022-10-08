namespace Ekstazz.Tools
{
    using System;

    public interface IProbabilityMap<T>
    {
        void Add(T t, float weight);
        void Add(T t);
        void Remove(T t);
        void Clear();
        IProbabilityMap<T> Where(Predicate<T> predicate);
        T Get();
    }
}