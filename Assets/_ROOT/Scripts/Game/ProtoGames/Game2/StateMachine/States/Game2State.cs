namespace Ekstazz.ProtoGames.Game2
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Ekstazz.ProtoGames.StateMachine;
    using ProtoGames.Character;

    
    public abstract class Game2State : State
    {
        protected CharacterView CharacterView;

        public abstract Game2States StateType { get; protected set; }
        
        private bool IsInitialized { get; set; }
        private List<Game2Transition> castTransitions = new List<Game2Transition>();
        
        
        protected virtual void Awake()
        {
            gameObject.SetActive(false);

            castTransitions = Array.ConvertAll(transitions, item => (Game2Transition)item).ToList();
        }

        public void Initialize(CharacterView characterView)
        {
            if (!IsInitialized)
            {
                IsInitialized = true;
                CharacterView = characterView;
                InitializeTransitions();
            }
        }

        private void InitializeTransitions()
        {
            foreach (var transition in castTransitions)
            {
                transition.Initialize(CharacterView);
            }
        }

        public abstract void Enter();

        public abstract void Exit();

        public bool HasTransition(out Game2State state)
        {
            foreach (var transition in castTransitions)
            {
                if (transition.IsValid())
                {
                    state = transition.To;
                    return true;
                }
            }
            state = null;
            return false;
        }
    }
    
    public enum Game2States
    {
        Unknown = 0,
        Idle = 1,
    }
}