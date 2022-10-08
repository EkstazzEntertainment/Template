namespace Ekstazz.ProtoGames.Flow
{
    using Ekstazz.Wallet;
    using Zenject;
    using Zenject.Extensions.Commands;

    public class ClearAccumulatedTemporaryCurrency : LockableCommand
    {
        [Inject] private IBackpack backpack;

        public override void Execute()
        {
            backpack.Clear();
        }
    }
}