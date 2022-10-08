namespace Ekstazz.ProtoGames.Game1
{
    using Ekstazz.ProtoGames.StateMachine;
    using ProtoGames.Character;
    using UnityEngine;

    public class Game1Transition : Transition
    {
        [SerializeField] private Game1Rule rule;
        [SerializeField] private Game1State state;

        public Game1State To => state;

        
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