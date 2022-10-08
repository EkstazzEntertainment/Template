namespace Ekstazz.ProtoGames.Game1.StateMachine
{
    using System;
    using Ekstazz.ProtoGames.StateMachine;
    using ProtoGames.Character;
    using UnityEngine;
    using Zenject;

    
    public class Game1StateMachine: StateMachine
    {
        [Inject] public SignalBus SignalBus { get; set; }
        
        [SerializeField] private Game1State initialState;
        
        public Game1States State => state != null ? state.StateType : Game1States.Unknown;
        public event Action<IState> OnStateChanged;
        public event Action OnStop;

        private Game1State state;

        
        public override void Init(CharacterView characterView)
        {
            state = initialState;
            state.Initialize(characterView);
            state.gameObject.SetActive(true);
            state.Enter();
        }

        public void Update()
        {
            if (state && state.HasTransition(out var nextState))
            {
                state.Exit();
                state.gameObject.SetActive(false);
                state = nextState;
                state.gameObject.SetActive(true);
                state.Enter();
                OnStateChanged?.Invoke(state);
            }
        }

        public void Stop()
        {
            if (state)
            {
                state.Exit();
                state.gameObject.SetActive(false);
            }
            OnStop?.Invoke();
            gameObject.SetActive(false);
        }
    }
}