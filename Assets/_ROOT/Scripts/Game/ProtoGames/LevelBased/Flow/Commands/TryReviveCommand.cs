namespace Ekstazz.LevelBased.Flow
{
    using Ekstazz.LevelBased.Views.Windows;
    using Ekstazz.Ui;
    using Logic;
    using Zenject;
    using Zenject.Extensions.Commands;

    
    public class TryReviveCommand : LockableCommand
    {
        [Inject] private UiBuilder uiBuilder;
        [Inject] private ITimeCounterController timeCounterController;
        [Inject] private ReviveController reviveController;


        public override void Execute()
        {
            if (reviveController.CanRevive())
            {
                Lock();
                CreateReviveWindow();
            }
        }

        private void CreateReviveWindow()
        {
            var options = CreateReviveWindowOptions();
            uiBuilder.CreateWindowWithOptions<ReviveWindow>(options);
        }

        private ReviveWindowOptions CreateReviveWindowOptions()
        {
            return new ReviveWindowOptions
            {
                OnReject = OnReject,
                OnSuccess = OnSuccess
            };
        }

        private void OnReject()
        {
            Unlock();
        }

        private void OnSuccess()
        {
            Revive();
            Cancel();
        }

        private void Revive()
        {
            timeCounterController.GamePlayTimeCounter.Resume();
            reviveController.ApplyReviving();
            // logic for restoring level state
        }
    }
}
