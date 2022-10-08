namespace Ekstazz.ProtoGames.Flow
{
    using System.Threading.Tasks;
    using Ekstazz.Wallet;
    using Zenject;
    using Zenject.Extensions.Commands;

    public class MoveEarnedCurrencyToWalletCommand : Command
    {
        [Inject] private IBackpack backpack;

        public override async Task Execute()
        {
            backpack.ApplyToWallet();
        }
    }
}