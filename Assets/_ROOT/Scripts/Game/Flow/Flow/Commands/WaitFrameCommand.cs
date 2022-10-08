namespace Ekstazz.Game.Flow
{
    using System.Threading.Tasks;
    using Ekstazz.Utils.Coroutine;
    using Zenject;
    using Zenject.Extensions.Commands;

    
    public class WaitFrameCommand : Command
    {
        [Inject] private ICoroutineProvider coroutineProvider;

        public override async Task Execute()
        {
            await 0;
        }
    }
}