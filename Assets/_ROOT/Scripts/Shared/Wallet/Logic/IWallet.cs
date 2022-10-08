namespace Ekstazz.Wallet
{
    using Ekstazz.Currencies;

    public interface IWallet
    {
        Amount GetAmountOf<T>() where T : ICurrencyType;
        Amount GetAmountOf(ICurrencyType type);
        void Add(Money money, string info = "Add");
        bool CanSpend(Money money);
        bool TrySpend(Money money, string info = "Spend");
    }

    public enum WalletSaveBehaviour
    {
        DoNotSave,
        SaveOnlyAfterSpent,
        AlwaysSave
    }
}
