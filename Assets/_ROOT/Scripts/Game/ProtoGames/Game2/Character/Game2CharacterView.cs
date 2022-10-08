namespace Ekstazz.ProtoGames.Game2.Character
{
    using Ekstazz.ProtoGames.StateMachine;
    using Ekstazz.ProtoGames.Character;
    using UnityEngine;
    using Zenject;


    public class Game2CharacterView : CharacterView
    {
        [Inject] public Game2CharacterMovement CharacterMovement { get; private set; }
        [Inject] public Game2CharacterRotation CharacterRotation { get; private set; }
        
        [SerializeField] private Game2ModelView modelView;
        
        public StateMachine StateMachine { get; protected set; }


        public override void StartCharacter()
        {
            base.StartCharacter();

            StateMachine.Init(this);
            
            InitMovement();
            InitRotation();
            InitAnimator();
        }
        
        private void InitMovement()
        {
            CharacterMovement.InitMovement();
        }

        private void InitRotation()
        {
            CharacterRotation.InitializeRotation();
        }

        private void InitAnimator()
        {
            modelView.Animator.InitAnimator();
        }

        public override void StopCharacter()
        {
            base.StopCharacter();
        }

        public override void SetUpStateMachine(StateMachine stateMachine)
        {
            base.SetUpStateMachine(stateMachine);

            StateMachine = stateMachine;
        }
    }
}
