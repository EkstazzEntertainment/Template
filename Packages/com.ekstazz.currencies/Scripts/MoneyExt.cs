namespace Ekstazz.Currencies
{
    public static class MoneyExt
    {
        public static MoneyFactory Factory { get; set; }
        
        public static Money Of<T>(this int amount)
            where T : class, ICurrencyType<T>, new()
        {
            var money = Factory.Create<T>(amount);
            return money;
        }
        
        public static Money Of<T>(this double amount)
            where T : class, ICurrencyType<T>, new()
        {
            var money = Factory.Create<T>(amount);
            return money;
        }
        
        public static Money Of<T>(this float amount)
            where T : class, ICurrencyType<T>, new()
        {
            var money = Factory.Create<T>(amount);
            return money;
        }
        
        public static Money Of<T>(this Amount amount)
            where T : class, ICurrencyType<T>, new()
        {
            var money = Factory.Create<T>(amount);
            return money;
        }


        public static Money Of(this Amount amount, ICurrencyType currency)
        {
            var money = Factory.Create(currency, amount);
            return money;
        }
    }
}