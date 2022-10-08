namespace Zenject.Extensions.Commands
{
    using System;
    using System.Collections.Generic;

    public class BindSignalCommandToBinder<TSignal>
    {
        private readonly DiContainer container;
        private readonly BindStatement bindStatement;
        private readonly SignalBindingBindInfo signalBindInfo;

        private readonly List<Type> commands = new List<Type>();
        private BindInfo bind;

        public BindSignalCommandToBinder(DiContainer container, SignalBindingBindInfo signalBindInfo)
        {
            this.container = container;
            this.signalBindInfo = signalBindInfo;
            bindStatement = container.StartBinding();
            bindStatement.SetFinalizer(new NullBindingFinalizer());
        }

        public BindSignalCommandToBinder<TSignal> To<TCommand>() where TCommand : ICommand
        {
            var type = typeof(TCommand);
            commands.Add(type);
            bind?.Reset();

            bind = container.Bind<IDisposable>()
                .To<SignalCommandWrapper>()
                .AsCached()
                .WithArguments(container, signalBindInfo, commands)
                .NonLazy().BindInfo;

            return this;
        }
    }
}