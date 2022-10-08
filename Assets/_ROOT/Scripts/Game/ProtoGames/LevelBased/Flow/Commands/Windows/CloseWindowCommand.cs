namespace Ekstazz.LevelBased.Flow
{
    using System.Linq;
    using System.Threading.Tasks;
    using Ekstazz.Ui;
    using Zenject;
    using Zenject.Extensions.Commands;

    
    public class CloseWindowCommand<T> : Command where T : Window
    {
        [Inject] private UiBuilder uiBuilder;

        public override async Task Execute()
        {
            var window = uiBuilder.CurrentWindows.FirstOrDefault(w => w is T);
            if (window)
            {
                window.Close();
            }
        }
    }
}
