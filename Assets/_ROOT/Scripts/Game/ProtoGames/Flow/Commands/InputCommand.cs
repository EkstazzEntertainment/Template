namespace Ekstazz.ProtoGames.Flow
{
    using System.Threading.Tasks;
    using Ekstazz.Input;
    using Zenject;
    using Zenject.Extensions.Commands;

    public class EnableInputCommand : Command
    {
        [Inject] private IInputProvider inputProvider;

        public override async Task Execute()
        {
            inputProvider.Enable();
        }
    }

    public class DisableInputCommand : Command
    {
        [Inject] private IInputProvider inputProvider;

        public override async Task Execute()
        {
            inputProvider.Disable();
        }
    }
}
