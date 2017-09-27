namespace BankSystem.Client.Commands
{
    using System;
    using System.Text;
    using Data;
    using Interfaces.Core;
    using IO;

    public class ListAccountsCommand : Command
    {
        public ListAccountsCommand(string[] commandArgs, IAuthenticationManager authenticationManager)
            : base(commandArgs, authenticationManager)
        {
        }

        public override string Execute()
        {
            // ListAccounts

            if (!this.AuthenticationManager.IsAuthenticated())
            {
                throw new InvalidOperationException(OutputMessages.UserShouldLogIn);
            }

            var builder = new StringBuilder();

            using (var context = new BankSystemDbContext())
            {
                var user = this.AuthenticationManager.GetCurrentUser();
                context.Users.Attach(user);

                builder.AppendLine("Saving Accounts:");
                foreach (var account in user.SavingsAccounts)
                {
                    builder.AppendLine(string.Format(OutputMessages.AccountBalance, account.AccountNumber, account.Balance));
                }

                builder.AppendLine("Checking Accounts:");
                foreach (var account in user.CheckingAccounts)
                {
                    builder.AppendLine(string.Format(OutputMessages.AccountBalance, account.AccountNumber, account.Balance));
                }
            }

            return builder.ToString().Trim();
        }
    }
}
