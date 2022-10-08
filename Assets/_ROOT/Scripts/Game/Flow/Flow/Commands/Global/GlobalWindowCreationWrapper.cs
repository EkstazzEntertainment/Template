namespace Ekstazz.Game.Flow
{
    using System.Linq;
    using Ekstazz.Ui;
    using Zenject;
    using Zenject.Extensions.Commands;

    
    public class GlobalWindowCreationWrapper<T> : LockableCommand where T : Window
    {
        [Inject] private UiBuilder uiBuilder;

        public override void Execute()
        {
            var window = uiBuilder.CurrentWindows.FirstOrDefault(w => w is T);
            if (!window)
            {
                uiBuilder.CreateWindow<T>(UiRootType.Global);
            }
        }
    }
}