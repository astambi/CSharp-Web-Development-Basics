namespace BankSystem.Client.Commands
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Data;
    using Interfaces.Core;
    using IO;
    using Models;

    public class RegisterCommand : Command
    {
        public RegisterCommand(string[] commandArgs, IAuthenticationManager authenticationManager)
            : base(commandArgs, authenticationManager)
        {
        }

        public override string Execute()
        {
            // Register <username> <password> <email>

            if (this.CommandArgs.Length != 3)
            {
                throw new ArgumentException(OutputMessages.InvalidInput);
            }

            if (this.AuthenticationManager.IsAuthenticated())
            {
                throw new InvalidOperationException(OutputMessages.UserShouldLogOut);
            }

            var username = this.CommandArgs[0];
            var password = this.CommandArgs[1];
            var email = this.CommandArgs[2];

            var user = new User()
            {
                Username = username,
                Password = password,
                Email = email
            };

            this.ValidateUser(user);

            using (var context = new BankSystemDbContext())
            {
                if (context.Users.Any(u => u.Username == username))
                {
                    throw new ArgumentException(OutputMessages.UsernameTaken);
                }

                if (context.Users.Any(u => u.Email == email))
                {
                    throw new ArgumentException(OutputMessages.EmailTaken);
                }

                context.Users.Add(user);
                context.SaveChanges();
            }

            return string.Format(OutputMessages.UserRegistered, username);
        }

        private void ValidateUser(User user)
        {
            bool isValid = true;
            string errors = string.Empty;

            if (user == null)
            {
                errors = OutputMessages.UserCannotBeNull;
                throw new ArgumentException(errors);
            }

            var usernameRegex = new Regex(@"^[a-zA-Z]+[a-zA-Z0-9]{2,}$");
            if (!usernameRegex.IsMatch(user.Username))
            {
                isValid = false;
                errors += OutputMessages.InvalidUsername + Environment.NewLine;
            }

            var passwordRegex = new Regex(@"^(?=[a-zA-Z0-9]*[A-Z])(?=[a-zA-Z0-9]*[a-z])(?=[a-zA-Z0-9]*[0-9])[a-zA-Z0-9]{6,}$");
            if (!passwordRegex.IsMatch(user.Password))
            {
                isValid = false;
                errors += OutputMessages.InvalidPassword + Environment.NewLine;
            }

            var emailRegex = new Regex(@"^([a-zA-Z0-9]+[-|_|\.]?)*[a-zA-Z0-9]+@([a-zA-Z0-9]+[-]?)*[a-zA-Z0-9]+\.([a-zA-Z0-9]+[-]?)*[a-zA-Z0-9]+$");
            if (!emailRegex.IsMatch(user.Email))
            {
                isValid = false;
                errors += OutputMessages.InvalidEmail;
            }

            if (!isValid)
            {
                throw new ArgumentException(errors.Trim());
            }
        }
    }
}
