namespace Ekstazz.ProtoGames.Flow
{
    using Zenject.Extensions.Commands;

    public class AwaitTimeCommand : LockableCommand
    {
        protected virtual float AwaitTime { get; private set; } = 0;


        public override async void Execute()
        {
            Lock();
            await AwaitTime;
            Unlock();
        }
    }
}
