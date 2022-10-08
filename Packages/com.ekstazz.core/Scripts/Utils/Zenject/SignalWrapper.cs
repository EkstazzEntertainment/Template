namespace Ekstazz.Utils
{
    using System.Threading.Tasks;
    using Zenject;
    using Zenject.Extensions.Commands;

    public class SignalWrapper<T> : Command where T : new()
    {
        [Inject]
        public SignalBus SignalBus { get; set; }

        public override async Task Execute()
        {
            SignalBus.AbstractFire<T>();
        }
    }
}