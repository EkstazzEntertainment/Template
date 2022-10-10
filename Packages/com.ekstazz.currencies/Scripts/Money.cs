namespace Ekstazz.Currencies
{
    using System;

    
    public readonly struct Money : IHasAmount
    {
        public Amount Amount => moneyAmount;
        private readonly Amount moneyAmount;
        
        public readonly ICurrencyType Type;

        
        public Money(ICurrencyType type, Amount amount)
        {
            Type = type;
            moneyAmount = amount;
        }

        public Money AddSame(in Money money2)
        {
            if (Type != money2.Type)
            {
                throw new Exception();
            }
            
            return new Money(Type, moneyAmount + money2.moneyAmount);
        }
    }
}