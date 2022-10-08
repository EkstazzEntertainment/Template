namespace Ekstazz.Game.Flow
{
    using System.Threading.Tasks;
    using Zenject;
    using Zenject.Extensions.Commands;

    public class SignalWrapper<T> : Command where T : new()
    {
        [Inject] private SignalBus signalBus;

        public override async Task Execute()
        {
            signalBus.AbstractFire<T>();
        }
    }
}