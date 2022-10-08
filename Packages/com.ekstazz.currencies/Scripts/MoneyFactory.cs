namespace Ekstazz.Currencies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class MoneyFactory
    {
        private readonly Dictionary<Type, ICurrencyType> currencies = new Dictionary<Type, ICurrencyType>();
        private readonly Dictionary<string, ICurrencyType> currenciesByName = new Dictionary<string, ICurrencyType>();
        
        public Money Create<T>(int i)
            where T : class, ICurrencyType<T>, new()
        {
            var type = typeof(T);
            if (!currencies.ContainsKey(type))
            {
                ThrowNotFound();
            }
            return new Money(currencies[type], i);
        }

        private void ThrowNotFound()
        {
            throw new KeyNotFoundException(
                $"No such currency found. Try these: {currenciesByName.Keys.Aggregate("", (s, s1) => s + ", \"" + s1 + "\"", s => s.Remove(0, 2))}, or add new currency using Register method.");
        }

        public Money Create<T>(in Amount i)
            where T : class, ICurrencyType<T>, new()
        {
            var type = typeof(T);
            if (!currencies.ContainsKey(type))
            {
                ThrowNotFound();
            }
            
            return new Money(currencies[type], i);
        }

        public void Register<T>()
            where T : class, ICurrencyType<T>, new()
        {
            var tmpInstance = new T();
            currencies.Add(typeof(T), tmpInstance.SingleInstance);
            currenciesByName.Add(tmpInstance.Name, tmpInstance.SingleInstance);
        }

        public Money Create(string type, in Amount amount)
        {
            return new Money(TypeOf(type), amount);
        }

        public ICurrencyType TypeOf(string typeName)
        {
            if (!currenciesByName.ContainsKey(typeName))
            {
                ThrowNotFound();
            }
            return currenciesByName[typeName];
        }

        public Money Create(in ICurrencyType currency, in Amount amount)
        {
            if (currencies[currency.GetType()] != currency)
            {
                throw new Exception("You should only use the reference to ICurrencyType provided by SingleInstance property.");
            } 
            
            return new Money(currency, amount);
        }
    }
}