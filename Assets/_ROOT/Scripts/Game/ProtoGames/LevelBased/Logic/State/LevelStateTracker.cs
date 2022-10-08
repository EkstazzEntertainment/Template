namespace Ekstazz.LevelBased.Logic.State
{
    using System;
    using Ekstazz.LevelBased.Flow.Signals;
    using Zenject;

    public class LevelStateTracker : ILevelStateProvider, IInitializable
    {
        [Inject] private SignalBus signalBus;

        public event Action<LevelState, LevelState> LevelStateChanged;
      
        public LevelState PreviousState { get; private set; } = LevelState.None;
        private LevelState currentState = LevelState.None;
        
        public LevelState CurrentState
        {
            get => currentState;
            private set
            {
                PreviousState = currentState;
                currentState = value;
                if (PreviousState != currentState)
                {
                    LevelStateChanged?.Invoke(PreviousState, currentState);
                }
            }
        }

        public void Initialize()
        {
            signalBus.Subscribe<PrepareLevel>(_ => SetStateTo(LevelState.Preparing));
            signalBus.Subscribe<ILevelReadyToStart>(_ => SetStateTo(LevelState.Prepared));
            signalBus.Subscribe<ILevelStarted>(_ => SetStateTo(LevelState.Started));
            signalBus.Subscribe<ILevelCompleting>(_ => SetStateTo(LevelState.Completing));
            signalBus.Subscribe<ILevelFailing>(_ => SetStateTo(LevelState.Failing));
            signalBus.Subscribe<ILevelCompleted>(_ => SetStateTo(LevelState.Completed));
            signalBus.Subscribe<ILevelFailed>(_ => SetStateTo(LevelState.Failed));
        }

        private void SetStateTo(LevelState targetState)
        {
            CurrentState = targetState;
        }
    }
}