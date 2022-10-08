namespace Ekstazz.Game.Flow
{
    using System.Threading.Tasks;
    using Zenject;
    using Zenject.Extensions.Commands;

    public class RemoveSplashCommand : LockableCommand
    {
        [Inject] private ISplash splash;

        public override void Execute()
        {
            var tcs = new TaskCompletionSource<bool>();
            splash.Remove(() => tcs.SetResult(true));
        }
    }
}