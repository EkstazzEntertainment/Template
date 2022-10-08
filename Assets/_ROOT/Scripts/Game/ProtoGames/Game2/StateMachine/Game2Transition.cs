namespace Ekstazz.ProtoGames.Game2
{
    using Ekstazz.ProtoGames.StateMachine;
    using ProtoGames.Character;
    using UnityEngine;

    
    public class Game2Transition : Transition
    {
        [SerializeField] private Game2Rule rule;
        [SerializeField] private Game2State state;

        public Game2State To => state;

        
        public void Initialize(CharacterView characterView)
        {
            rule.Initialize(characterView);
            state.Initialize(characterView);
        }

        public bool IsValid()
        {
            return rule.IsValid();
        }
    }
}