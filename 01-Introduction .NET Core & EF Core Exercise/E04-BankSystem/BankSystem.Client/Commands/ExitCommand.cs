namespace BankSystem.Client.Commands
{
    using System;
    using Interfaces.Core;

    class ExitCommand : Command
    {
        public ExitCommand(string[] commandArgs, IAuthenticationManager authenticationManager)
            : base(commandArgs, authenticationManager)
        {
        }

        public override string Execute()
        {
            Environment.Exit(0);

            return string.Empty;
        }
    }
}
