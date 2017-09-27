namespace BankSystem.Client.Commands
{
    using BankSystem.Client.Interfaces.Commands;
    using BankSystem.Client.Interfaces.Core;

    public abstract class Command : ICommand
    {
        public Command(string[] commandArgs, IAuthenticationManager authenticationManager)
        {
            this.CommandArgs = commandArgs;
            this.AuthenticationManager = authenticationManager;
        }

        protected string[] CommandArgs { get; }

        protected IAuthenticationManager AuthenticationManager { get; }

        public abstract string Execute();
    }
}
