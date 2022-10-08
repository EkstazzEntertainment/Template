namespace Ekstazz.ProtoGames.Game1.Input
{
    using Character;
    using Ekstazz.Input;
    using Ekstazz.Input.Drags;
    using Flow;
    using ProtoGames.Level;
    using Zenject;
    using Vector2 = UnityEngine.Vector2;

    
    public class Game1Input : IInitializable
    {
        [Inject] private IInputProvider inputProvider;
        [Inject] private SignalBus signalBus;
        [Inject] private ILevelViewProvider<Game1LevelView> levelView;

        private Game1CharacterView CharacterView => levelView.LevelView.PlayerView as Game1CharacterView;


        public void Initialize()
        {
            signalBus.Subscribe<Game1Started>(Subscribe);
            signalBus.Subscribe<Game1Ending>(Unsubscribe);
        }

        private void Subscribe()
        {
            inputProvider.OnDragStart += OnDragStart;
            inputProvider.OnDrag += OnDrag;
            inputProvider.OnDragEnd += OnDragEnd;
        }

        private void OnDragStart(Drag dragInfo)
        {
        }

        private void OnDrag(Drag dragInfo)
        {
            CharacterView.CharacterMovement.SetMovementParameters(dragInfo.OverallDelta);
            CharacterView.CharacterRotation.SetMovementParameters(dragInfo.OverallDelta);
            CharacterView.CharacterMovement.ActivateMovement();
            CharacterView.CharacterRotation.ActivateRotation();
        }

        private void OnDragEnd(Drag dragInfo)
        {
            CharacterView.CharacterMovement.SetMovementParameters(Vector2.zero);
            CharacterView.CharacterMovement.DeactivateMovement();
            CharacterView.CharacterRotation.DeactivateRotation();
        }

        private void Unsubscribe()
        {
            inputProvider.OnDrag -= OnDrag;
            inputProvider.OnDragEnd -= OnDragEnd;
            inputProvider.OnDragStart -= OnDragStart;
        }
    }
}