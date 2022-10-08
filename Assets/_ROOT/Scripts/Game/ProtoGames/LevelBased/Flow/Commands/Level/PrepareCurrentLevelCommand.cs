namespace Ekstazz.LevelBased.Flow
{
    using System.Threading.Tasks;
    using Ekstazz.LevelBased.Logic;
    using Signals;
    using Zenject;
    using Zenject.Extensions.Commands;

    
    public class PrepareCurrentLevelCommand : Command
    {
        [Inject] private ILevelProvider levelProvider;
        [Inject] private SignalBus signalBus;

        
        public override async Task Execute()
        {
            signalBus.AbstractFire(new PrepareLevel { Level = levelProvider.CurrentLevel }); 
            await WaitLevelLoading();
        }

        private async Task WaitLevelLoading()
        {
            var tcs = new TaskCompletionSource<bool>();
            signalBus.Subscribe<ILevelReadyToStart>(OnLevelReadyToStart);
            await tcs.Task;

            void OnLevelReadyToStart(ILevelReadyToStart levelReadyToStart)
            {
                signalBus.Unsubscribe<ILevelReadyToStart>(OnLevelReadyToStart);
                tcs.SetResult(true);
            }
        }
    }
}