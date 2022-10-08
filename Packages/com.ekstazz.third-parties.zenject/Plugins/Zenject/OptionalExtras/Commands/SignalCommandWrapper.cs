namespace Zenject.Extensions.Commands
{
    using System;
    using System.Collections.Generic;

    public class SignalCommandWrapper : IDisposable
    {
        private readonly DiContainer container;
        private readonly SignalBus signalBus;
        private readonly Type signalType;
        private readonly object identifier;

        private readonly List<Type> commands;

        public SignalCommandWrapper(DiContainer container, SignalBindingBindInfo bindInfo, List<Type> commands,
            SignalBus signalBus)
        {
            this.container = container;
            signalType = bindInfo.SignalType;
            identifier = bindInfo.Identifier;
            this.commands = commands;
            this.signalBus = signalBus;

            signalBus.SubscribeId(bindInfo.SignalType, identifier, OnSignalFired);
        }

        private void OnSignalFired(object signal)
        {
            var sequencer = new Sequencer(container, commands, signal);
            sequencer.Execute();
        }

        public void Dispose()
        {
            signalBus.UnsubscribeId(signalType, identifier, OnSignalFired);
        }
    }
}