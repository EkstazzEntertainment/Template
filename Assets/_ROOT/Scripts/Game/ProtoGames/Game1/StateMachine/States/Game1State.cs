namespace Ekstazz.ProtoGames.Game1
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Ekstazz.ProtoGames.StateMachine;
    using ProtoGames.Character;

    
    public abstract class Game1State : State
    {
        protected CharacterView CharacterView;

        public abstract Game1States StateType { get; protected set; }
        
        private bool IsInitialized { get; set; }
        private List<Game1Transition> castTransitions = new List<Game1Transition>();
        
        
        protected virtual void Awake()
        {
            gameObject.SetActive(false);

            castTransitions = Array.ConvertAll(transitions, item => (Game1Transition)item).ToList();
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

        public bool HasTransition(out Game1State state)
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
    
    public enum Game1States
    {
        Unknown = 0,
        Idle = 1,
    }
}