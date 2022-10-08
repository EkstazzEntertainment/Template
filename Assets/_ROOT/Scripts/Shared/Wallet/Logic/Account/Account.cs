namespace Ekstazz.Wallet
{
    using System;
    using Ekstazz.Currencies;
    using Zenject;

    
    public class Account
    {
        [Inject] private SignalBus signalBus;

        public ICurrencyType Currency { get; }
        public Amount Amount { get; private set; }
        public Amount Spent { get; private set; }

        public Account(ICurrencyType currency, int amount)
        {
            Currency = currency;
            Amount = amount;
        }

        public Account(AccountSave save, MoneyFactory factory)
        {
            Currency = factory.TypeOf(save.Type);
            Amount = save.Amount;
            Spent = save.Spent;
        }

        
        public bool CanSpent(in Amount amount)
        {
            return amount <= Amount;
        }

        public void Add(Money money, string info)
        {
            ThrowIfWrongType(money);

            Amount += money.Amount;
            signalBus.Fire(new TransactionCompleted
            {
                Value = Amount,
                Type = Currency,
                TotalSpent = Spent,
                Transaction = new Transaction(info, TransactionType.Received, money.Amount)
            });
        }

        public void Spend(Money money, string info)
        {
            ThrowIfWrongType(money);

            Amount -= money.Amount;
            Spent += money.Amount;
            signalBus.Fire(new TransactionCompleted
            {
                Value = Amount,
                Type = Currency,
                TotalSpent = Spent,
                Transaction = new Transaction(info, TransactionType.Spend, -money.Amount)
            });
        }

        public AccountSave ToSave()
        {
            return new AccountSave
            {
                Type = Currency.Name,
                Amount = Amount,
                Spent = Spent
            };
        }

        private void ThrowIfWrongType(Money currency)
        {
            if (currency.Type != Currency)
            {
                throw new Exception(
                    $"Can not operate with currency of type {currency.Type}. Account currency type: {Currency}");
            }
        }
    }
}