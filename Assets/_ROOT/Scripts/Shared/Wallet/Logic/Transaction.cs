namespace Ekstazz.Wallet
{
    using Ekstazz.Currencies;

    public sealed class Transaction
    {
        public readonly string Info;
        public readonly TransactionType Type;
        public readonly Amount Amount;
        
        public Transaction(string info, TransactionType type, Amount amount)
        {
            Info = info;
            Type = type;
            Amount = amount;
        }
    }

    public enum TransactionType
    {
        Received,
        Spend
    }
}