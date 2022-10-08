namespace Ekstazz.LevelBased.Views.Windows
{
    using Ui;
    using Ekstazz.LevelBased.Flow.Signals;
    using Zenject;

    
    public abstract class LevelCompletedWindow : Window
    {
        [Inject] public SignalBus SignalBus { get; set; }

        protected bool IsClosing;
        
        
        public virtual void Next()
        {
            
        }
    }
    
    public class LevelCompletedWindow<T> : LevelCompletedWindow where T : ILevelCompleted, new()
    {
        public override void Next()
        {
            if (IsClosing)
            {
                return;
            }
            
            IsClosing = true;
            SignalBus.AbstractFire<T>();
            Close();
        }
    }
}
