namespace Ekstazz.LevelBased.Views.Windows
{
    using Ekstazz.LevelBased.Flow.Signals;
    using Ui;

    public abstract class GameWindow : Window
    {
       
    }
    
    public class GameWindow<T> : GameWindow where T : ILevelRestarting, new()
    {
        
    }
}