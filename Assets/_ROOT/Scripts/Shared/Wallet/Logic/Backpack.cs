namespace Ekstazz.Wallet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Ekstazz.Currencies;
    using Zenject;

    
    public interface IBackpack
    {
        event Action<Money> OnUpdate;
        void Add(Money money);
        Amount GetAmountOf(ICurrencyType currency);
        bool HasAmountOf(Money money);
        bool TrySpend(Money money);
        List<Money> GetContent();
        void Clear();
        void ApplyToWallet();
    }

    
    public class Backpack : IBackpack
    {
        [Inject] private IWallet wallet;

        private const string Info = "Backpack";

        private readonly IDictionary<ICurrencyType, Amount> accounts = new Dictionary<ICurrencyType, Amount>();

        public event Action<Money> OnUpdate;

        
        public void Add(Money money)
        {
            var amount = GetAmountOf(money.Type) + money.Amount;
            accounts[money.Type] = amount;
            OnUpdate?.Invoke(amount.Of(money.Type));
        }

        public Amount GetAmountOf(ICurrencyType currency)
        {
            return accounts.TryGetValue(currency, out var amount) ? amount : CreateEmptyAccount(currency);
        }

        private Amount CreateEmptyAccount(ICurrencyType currency)
        {
            return accounts[currency] = 0;
        }

        public bool HasAmountOf(Money money)
        {
            return GetAmountOf(money.Type) >= money.Amount;
        }

        public bool TrySpend(Money money)
        {
            if (HasAmountOf(money))
            {
                accounts[money.Type] -= money.Amount;
                OnUpdate?.Invoke(accounts[money.Type].Of(money.Type));
                return true;
            }
            return false;
        }

        public List<Money> GetContent()
        {
            return accounts
                .Select(pair => pair.Value.Of(pair.Key))
                .ToList();
        }

        public void Clear()
        {
            accounts.Clear();
        }

        public void ApplyToWallet()
        {
            var content = GetContent();
            Clear();
            foreach (var money in content)
            {
                wallet.Add(money, Info);
                OnUpdate?.Invoke(((Amount)0).Of(money.Type));
            }
        }
    }
}
