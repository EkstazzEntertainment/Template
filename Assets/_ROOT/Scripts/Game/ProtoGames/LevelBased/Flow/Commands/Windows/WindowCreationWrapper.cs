namespace Ekstazz.LevelBased.Flow
{
    using System.Linq;
    using System.Threading.Tasks;
    using Ekstazz.Ui;
    using ProtoGames;
    using Zenject;
    using Zenject.Extensions.Commands;

    
    public class WindowCreationWrapper<T> : Command where T : Window
    {
        [Inject] private LevelGameProvider levelGameProvider;

        
        public override async Task Execute()
        {
            var uiBuilder = levelGameProvider.CurrentLevelGame.UiBuilder;
            var window = uiBuilder.CurrentWindows.FirstOrDefault(w => w is T);
            if (!window)
            {
                uiBuilder.CreateWindow<T>();
            }
        }
    }
}