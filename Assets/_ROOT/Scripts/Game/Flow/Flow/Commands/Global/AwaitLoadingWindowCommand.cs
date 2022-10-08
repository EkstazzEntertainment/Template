namespace Ekstazz.Game.Flow
{
    using System.Linq;
    using Ekstazz.Ui;
    using Zenject;
    using Zenject.Extensions.Commands;

    public class AwaitLoadingWindowCommand : LockableCommand
    {
        [Inject] private UiBuilder uiBuilder;

        public override void Execute()
        {
            var window = uiBuilder.CurrentWindows.FirstOrDefault(x => x is LoadingWindow) as LoadingWindow;
            if (window)
            {
                if (!window.IsFinished)
                {
                    window.ProgressFinished += Unlock;
                    Lock();
                }
            }
        }
    }
}