namespace Ekstazz.Wallet
{
    using Ekstazz.Currencies;

    public class TransactionCompleted
    {
        public ICurrencyType Type { get; set; }
        public Amount Value { get; set; }
        public Amount TotalSpent { get; set; }
        public Transaction Transaction { get; internal set; }
    }
}