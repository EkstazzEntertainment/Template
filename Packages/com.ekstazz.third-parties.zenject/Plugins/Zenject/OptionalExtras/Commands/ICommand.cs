namespace Zenject.Extensions.Commands
{
    using System;
    using System.Threading.Tasks;

    public interface ICommand
    {
        bool IsCanceled { get; }

        Task Execute();
    }

    public abstract class Command : ICommand
    {
        public bool IsCanceled { get; private set; }

        public abstract Task Execute();

        protected void Cancel()
        {
            IsCanceled = true;
        }
    }

    public abstract class LockableCommand : ICommand
    {
        public bool IsCanceled { get; private set; }

        private readonly TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
        private bool isExecutionLocked;

        async Task ICommand.Execute()
        {
            Execute();
            if (isExecutionLocked)
            {
                await WaitForRelease();
            }
        }

        public abstract void Execute();

        private async Task WaitForRelease()
        {
            await tcs.Task;
        }

        public void Lock()
        {
            isExecutionLocked = true;
        }

        public void Unlock()
        {
            tcs.TrySetResult(true);
        }

        protected void Cancel()
        {
            IsCanceled = true;
        }
    }
}
