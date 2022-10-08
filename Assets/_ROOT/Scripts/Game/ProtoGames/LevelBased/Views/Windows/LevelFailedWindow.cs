namespace Ekstazz.LevelBased.Views.Windows
{
    using Ekstazz.LevelBased.Flow.Signals;
    using Ui;
    using Zenject;

    
    public abstract class LevelFailedWindow : Window
    {
        [Inject] private protected SignalBus SignalBus;

        private bool isClosing;
        
        protected abstract void FireLevelFailed();

        
        public void Restart()
        {
            if (isClosing)
            {
                return;
            }
            
            isClosing = true;
            FireLevelFailed();
            Close();
        }
    }
    
    public class LevelFailedWindow<T> : LevelFailedWindow where T : ILevelFailed, new()
    {
        protected override void FireLevelFailed()
        {
            SignalBus.AbstractFire<T>();
        }
    }
}