namespace Ekstazz.ProtoGames.Currency
{
    using Ekstazz.Currencies;

    public class Coin : CurrencyType<Coin>
    {
        public override string Name => nameof(Coin);
    }
}
