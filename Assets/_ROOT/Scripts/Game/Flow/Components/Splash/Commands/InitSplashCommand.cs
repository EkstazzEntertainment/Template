namespace Ekstazz.Game.Flow
{
    using System.Threading.Tasks;
    using Zenject;
    using Zenject.Extensions.Commands;

    public class InitSplashCommand : Command
    {
        [Inject] private ISplash splash;
        
        public override async Task Execute()
        {
            splash.Init();
        }
    }
}