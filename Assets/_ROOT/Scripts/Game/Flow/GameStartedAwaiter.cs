namespace Ekstazz.Game.Flow
{
    using System;
    using System.Collections.Generic;
    using Zenject;

    public class GameStartedAwaiter : IInitializable
    {
        [Inject] private SignalBus signalBus;

        private readonly List<Action> actions = new List<Action>();
        private bool gameStarted;

        
        public void Initialize()
        {
            signalBus.Subscribe<GameSceneLoaded>(OnGameSceneLoaded);
        }

        private void OnGameSceneLoaded()
        {
            gameStarted = true;
            foreach (var action in actions)
            {
                action.Invoke();
            }

            actions.Clear();
        }

        public void ExecuteAfterGameStarted(Action action)
        {
            if (gameStarted)
            {
                action.Invoke();
            }
            else
            {
                actions.Add(action);
            }
        }
    }
}