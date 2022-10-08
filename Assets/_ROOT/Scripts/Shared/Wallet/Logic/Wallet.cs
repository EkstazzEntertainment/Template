namespace Ekstazz.Wallet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Configs;
    using Ekstazz.Currencies;
    using Ekstazz.Saves.General.Components;
    using Zenject;

    
    internal class Wallet : SaveableComponent<WalletSave>, IWallet
    {
        [Inject] private IAccountFactory accountFactory;
        [Inject] private WalletConfig config;
        [Inject] private MoneyFactory moneyFactory;

        private readonly IDictionary<ICurrencyType, Account> accounts = new Dictionary<ICurrencyType, Account>();

        private WalletSaveBehaviour SaveBehaviour => config.Defaults.SaveBehaviour;

        
        public Amount GetAmountOf<T>() where T : ICurrencyType
        {
            var instance = Activator.CreateInstance<T>();
            var currencyType = moneyFactory.TypeOf(instance.Name);
            return GetAmountOf(currencyType);
        }

        public Amount GetAmountOf(ICurrencyType type)
        {
            var account = GetAccount(type);
            return account?.Amount ?? 0;
        }

        public void Add(Money money, string info = "Add")
        {
            var account = GetOrCreateAccount(money.Type);
            account.Add(money, info);
            TrySave(TransactionType.Received);
        }

        public bool CanSpend(Money money)
        {
            return GetAmountOf(money.Type) >= money.Amount;
        }

        public bool TrySpend(Money money, string info = "Spend")
        {
            var account = GetOrCreateAccount(money.Type);
            if (account.CanSpent(money.Amount))
            {
                account.Spend(money, info);
                TrySave(TransactionType.Spend);
                return true;
            }

            return false;
        }

        private Account GetOrCreateAccount(ICurrencyType currency)
        {
            var account = GetAccount(currency);
            return account ?? CreateEmptyAccount(currency);
        }

        private Account GetAccount(ICurrencyType currency)
        {
            return accounts.TryGetValue(currency, out var account) ? account : null;
        }

        private Account CreateEmptyAccount(ICurrencyType currency)
        {
            var account = accountFactory.Create(currency, 0);
            accounts[currency] = account;
            return account;
        }

        private void TrySave(TransactionType transaction)
        {
            switch (SaveBehaviour)
            {
                case WalletSaveBehaviour.SaveOnlyAfterSpent when transaction == TransactionType.Spend:
                    ScheduleSave();
                    break;
                case WalletSaveBehaviour.AlwaysSave:
                    ScheduleSave();
                    break;
            }
        }

        protected override WalletSave PrepareInitialSave()
        {
            var save = new WalletSave
            {
                Accounts = new List<AccountSave>()
            };

            foreach (var pair in config.Defaults.Currencies)
            {
                var accountSave = new AccountSave
                {
                    Amount = pair.Value,
                    Type = pair.Key
                };

                save.Accounts.Add(accountSave);
            }

            return save;
        }

        public override WalletSave Serialize()
        {
            return new WalletSave
            {
                Accounts = accounts.Values.Select(account => account.ToSave()).ToList()
            };
        }

        public override void Deserialize(WalletSave save, DateTime lastSaveTime)
        {
            accounts.Clear();
            foreach (var accountSave in save.Accounts)
            {
                var account = accountFactory.Create(accountSave);
                accounts[moneyFactory.TypeOf(accountSave.Type)] = account;
            }
        }
    }
}
