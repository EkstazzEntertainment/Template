namespace Ekstazz.LevelBased.Logic
{
    using Flow.Signals;
    using Zenject;

    public interface ITimeCounterController
    {
        ITimeCounter GamePlayTimeCounter { get; }
        ITimeCounter TotalTimeCounter { get; }
    }

    public class TimeCounterController : ITimeCounterController, IInitializable
    {
        [Inject] private SignalBus signalBus;

        [Inject] public ITimeCounter GamePlayTimeCounter { get; set; }
        [Inject] public ITimeCounter TotalTimeCounter  { get; set; }


        public void Initialize()
        {
            Subscribe();
            SetLevelTimer();
        }

        private void Subscribe()
        {
            signalBus.Subscribe<ILevelStarted>(OnLevelStarted);
            signalBus.Subscribe<IProcessFail>(OnProcessFail);
            signalBus.Subscribe<ILevelEnding>(OnLevelEnding);
        }

        private void SetLevelTimer()
        {
            TotalTimeCounter.Init();
        }

        private void OnLevelStarted()
        {
            GamePlayTimeCounter.Init();
        }

        private void OnProcessFail()
        {
            GamePlayTimeCounter.Pause();
        }

        private void OnLevelEnding()
        {
            GamePlayTimeCounter.Pause();
        }
    }
}