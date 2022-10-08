namespace Ekstazz.Game.Flow
{
    using Zenject.Extensions.Commands;

    public class WaitAfterLoadingFinishedCommand : LockableCommand
    {
        public override async void Execute()
        {
            Lock();
            await 0.5f;
            Unlock();
        }
    }
}
