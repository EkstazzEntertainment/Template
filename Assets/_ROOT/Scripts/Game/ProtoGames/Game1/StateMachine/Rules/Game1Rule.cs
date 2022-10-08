namespace Ekstazz.ProtoGames.Game1
{
    using Ekstazz.ProtoGames.StateMachine;
    using ProtoGames.Character;
    using UnityEngine;

    public abstract class Game1Rule : MonoBehaviour, IRule
    {
        protected CharacterView CharacterView;

        public  void Initialize(CharacterView characterView)
        {
            CharacterView = characterView;
        }

        public abstract bool IsValid();
    }
}