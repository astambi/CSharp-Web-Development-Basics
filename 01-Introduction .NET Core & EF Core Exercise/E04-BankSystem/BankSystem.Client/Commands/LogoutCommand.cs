namespace BankSystem.Client.Commands
{
    using System;
    using Interfaces.Core;
    using IO;

    public class LogoutCommand : Command
    {
        public LogoutCommand(string[] commandArgs, IAuthenticationManager authenticationManager)
            : base(commandArgs, authenticationManager)
        {
        }

        public override string Execute()
        {
            // Logout

            if (!this.AuthenticationManager.IsAuthenticated())
            {
                throw new InvalidOperationException(OutputMessages.UserCannotLogOut);
            }

            var user = this.AuthenticationManager.GetCurrentUser();
            this.AuthenticationManager.Logout();

            return string.Format(OutputMessages.UserLoggedOut, user.Username);
        }
    }
}
