namespace BankSystem.Client.Core
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Interfaces.Commands;
    using Interfaces.Core;
    using IO;

    public class CommandProcessor : ICommandProcessor
    {
        private const string CommandSuffix = "Command";

        private IAuthenticationManager authenticationManager;

        public CommandProcessor(IAuthenticationManager authenticationManager)
        {
            this.authenticationManager = authenticationManager;
        }

        public string ProcessCommand(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return OutputMessages.InvalidCommand;
            }

            var commandArgs = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            var command = commandArgs[0];
            commandArgs = commandArgs.Skip(1).ToArray();

            Type commandType = Assembly
                               .GetExecutingAssembly()
                               .GetTypes()
                               .FirstOrDefault(t => t.Name == command + CommandSuffix);

            var commandParams = new object[] { commandArgs, this.authenticationManager };

            var cmd = (ICommand)Activator.CreateInstance(commandType, commandParams);

            if (cmd == null)
            {
                return OutputMessages.InvalidCommand;
            }

            return cmd.Execute();
        }
    }
}
