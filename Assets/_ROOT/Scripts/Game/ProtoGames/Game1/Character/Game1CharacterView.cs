namespace Ekstazz.ProtoGames.Game1.Character
{
    using Ekstazz.ProtoGames.StateMachine;
    using Ekstazz.ProtoGames.Character;
    using UnityEngine;
    using Zenject;

    
    public class Game1CharacterView : CharacterView
    {
        [Inject] public Game1CharacterMovement CharacterMovement { get; private set; }
        [Inject] public Game1CharacterRotation CharacterRotation { get; private set; }

        [SerializeField] private Game1ModelView modelView;
        
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