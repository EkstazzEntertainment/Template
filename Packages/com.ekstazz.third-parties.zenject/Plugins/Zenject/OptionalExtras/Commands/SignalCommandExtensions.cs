namespace Zenject.Extensions.Commands
{
    public static class SignalCommandExtensions
    {
        public static BindSignalCommandToBinder<TSignal> BindSignalToCommand<TSignal>(this DiContainer container)
        {
            var signalBindInfo = new SignalBindingBindInfo(typeof(TSignal));

            return new BindSignalCommandToBinder<TSignal>(container, signalBindInfo);
        }
    }
}