namespace Ekstazz.ProtoGames.Character
{
    using Ekstazz.ProtoGames.StateMachine;
    using UnityEngine;

    public class CharacterView : MonoBehaviour, ICharacterView
    {
        [field: SerializeField] public Transform ModelRoot { get; private set; }
        
        
        public virtual void StartCharacter()
        {
            
        }
        
        public virtual void StopCharacter()
        {
            
        }
        
        public virtual void SetUpStateMachine(StateMachine stateMachine)
        {
            
        }
    }
}