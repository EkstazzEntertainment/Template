namespace Zenject.Extensions.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class Sequencer
    {
        private readonly DiContainer container;
        private readonly List<Type> commands;
        private readonly object signal;

        private int currentCommandIndex;

        public Sequencer(DiContainer container, List<Type> commands, object signal)
        {
            this.container = container;
            this.commands = commands;
            this.signal = signal;
        }

        public void Execute()
        {
            if (commands.Any())
            {
                InvokeCommand(commands[0]);
            }
        }

        private async void InvokeCommand(Type type)
        {
            try
            {
                var command = CreateCommand(type);
                await command.Execute();
                if (command.IsCanceled)
                {
                    Debug.LogWarning(
                        $"Command: {command} was canceled. Commands:\n{GetLeftCommandNames()}\nwere skipped");
                    return;
                }
                ExecuteNext();
            }
            catch (Exception e)
            {
                Debug.LogError(
                    $"Error occurred while processing command for signal: {signal}. See next log for more details");
                Debug.LogException(e);
                throw;
            }
        }

        private ICommand CreateCommand(Type type)
        {
            return (ICommand)container.Instantiate(type, new[] { signal });
        }

        private void ExecuteNext()
        {
            currentCommandIndex++;
            if (currentCommandIndex < commands.Count)
            {
                InvokeCommand(commands[currentCommandIndex]);
            }
        }

        private string GetLeftCommandNames()
        {
            return string.Join("\n", commands.Skip(currentCommandIndex + 1));
        }
    }
}
