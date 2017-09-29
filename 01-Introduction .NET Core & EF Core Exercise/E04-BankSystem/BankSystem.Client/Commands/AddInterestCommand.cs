namespace BankSystem.Client.Commands
{
    using System;
    using System.Linq;
    using Data;
    using Interfaces.Core;
    using IO;

    public class AddInterestCommand : Command
    {
        public AddInterestCommand(string[] commandArgs, IAuthenticationManager authenticationManager)
            : base(commandArgs, authenticationManager)
        {
        }

        public override string Execute()
        {
            // AddInterest <Account number> 

            if (this.CommandArgs.Length != 1)
            {
                throw new ArgumentException(OutputMessages.InvalidInput);
            }

            if (!this.AuthenticationManager.IsAuthenticated())
            {
                throw new InvalidOperationException(OutputMessages.UserShouldLogIn);
            }

            var accountNumber = this.CommandArgs[0];

            decimal currentBalance;

            using (var context = new BankSystemDbContext())
            {
                var user = this.AuthenticationManager.GetCurrentUser();
                context.Users.Attach(user);

                var savingAccount = user
                                    .SavingsAccounts
                                    .FirstOrDefault(a => a.AccountNumber == accountNumber);

                if (savingAccount == null)
                {
                    throw new ArgumentException(OutputMessages.InvalidAccount);
                }

                savingAccount.AddInterest();
                context.SaveChanges();
                currentBalance = savingAccount.Balance;
            }

            return string.Format(OutputMessages.AddedInterest, accountNumber, currentBalance);
        }
    }
}
