namespace Ekstazz.Wallet
{
    using Ekstazz.Currencies;
    using Zenject;

    
    public interface IAccountFactory
    {
        Account Create(AccountSave saveModel);
        Account Create(ICurrencyType currency, int amount);
    }

    public class StrangeAccountFactory : IAccountFactory
    {
        [Inject] private DiContainer container;

        
        public Account Create(AccountSave saveModel)
        {
            var account = new Account(saveModel, container.Resolve<MoneyFactory>());
            container.Inject(account);
            return account;
        }

        public Account Create(ICurrencyType currency, int amount)
        {
            var account = new Account(currency, amount);
            container.Inject(account);
            return account;
        }
    }
}