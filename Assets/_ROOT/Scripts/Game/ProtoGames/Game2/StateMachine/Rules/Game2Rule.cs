namespace Ekstazz.ProtoGames.Game2
{
    using Ekstazz.ProtoGames.StateMachine;
    using ProtoGames.Character;
    using UnityEngine;

    public abstract class Game2Rule : MonoBehaviour, IRule
    {
        protected CharacterView CharacterView;

        public  void Initialize(CharacterView characterView)
        {
            CharacterView = characterView;
        }

        public abstract bool IsValid();
    }
}