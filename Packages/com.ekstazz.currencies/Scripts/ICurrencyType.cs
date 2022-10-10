namespace Ekstazz.Currencies
{
    public abstract class CurrencyType<T> : ICurrencyType<T> where T : CurrencyType<T>, new()
    {
        private static readonly CurrencyType<T> Instance = new T();
        public T SingleInstance => (T)Instance;
        public abstract string Name { get; }
    }
    
    public interface ICurrencyType<T> : ICurrencyType where T : ICurrencyType<T>
    {
        T SingleInstance { get; }
    }

    public interface ICurrencyType
    {
        string Name { get; }
    }

    public interface IHasPrice
    {
        Money Price { get; }
    }
}