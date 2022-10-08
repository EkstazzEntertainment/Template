namespace Ekstazz.ProtoGames.Game2.Input
{
    using Character;
    using Ekstazz.Input;
    using Ekstazz.Input.Drags;
    using Flow;
    using ProtoGames.Level;
    using Zenject;
    using Vector2 = UnityEngine.Vector2;

    
    public class Game2Input : IInitializable
    {
        [Inject] private IInputProvider inputProvider;
        [Inject] private SignalBus signalBus;
        [Inject] private ILevelViewProvider<Game2LevelView> levelView;

        private Game2CharacterView CharacterView => levelView.LevelView.PlayerView as Game2CharacterView;


        public void Initialize()
        {
            signalBus.Subscribe<Game2Started>(Subscribe);
            signalBus.Subscribe<Game2Ending>(Unsubscribe);
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