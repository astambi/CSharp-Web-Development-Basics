namespace BankSystem.Client.Commands
{
    using System;
    using System.Linq;
    using Data;
    using Interfaces.Core;
    using IO;

    public class LoginCommand : Command
    {
        public LoginCommand(string[] commandArgs, IAuthenticationManager authenticationManager)
            : base(commandArgs, authenticationManager)
        {
        }

        public override string Execute()
        {
            // Login <username> <password> 

            if (this.CommandArgs.Length != 2)
            {
                throw new ArgumentException(OutputMessages.InvalidInput);
            }

            if (this.AuthenticationManager.IsAuthenticated())
            {
                throw new InvalidOperationException(OutputMessages.UserShouldLogOut);
            }

            var username = this.CommandArgs[0];
            var password = this.CommandArgs[1];

            using (var context = new BankSystemDbContext())
            {
                var user = context.Users
                           .FirstOrDefault(u => u.Username == username &&
                                                u.Password == password);

                if (user == null)
                {
                    throw new ArgumentException(OutputMessages.InvalidUsernameOrPassword);
                }

                this.AuthenticationManager.Login(user);
            }

            return string.Format(OutputMessages.UserLoggedIn, username);
        }
    }
}
