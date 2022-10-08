namespace Ekstazz.ProtoGames.Level
{
    using Ekstazz.Input;
    using Ekstazz.Input.Drags;
    using LevelBased.Flow.Signals;
    using Zenject;

    
    public class PlayerInput : IInitializable
    {
        [Inject] private IInputProvider inputProvider;
        [Inject] private SignalBus signalBus;

        
        public void Initialize()
        {
            signalBus.Subscribe<ILevelStarted>(Subscribe);
            signalBus.Subscribe<ILevelEnding>(Unsubscribe);
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
            
        }

        private void OnDragEnd(Drag dragInfo)
        {
            
        }

        private void Unsubscribe()
        {
            inputProvider.OnDrag -= OnDrag;
            inputProvider.OnDragEnd -= OnDragEnd;
            inputProvider.OnDragStart -= OnDragStart;
        }
    }
}