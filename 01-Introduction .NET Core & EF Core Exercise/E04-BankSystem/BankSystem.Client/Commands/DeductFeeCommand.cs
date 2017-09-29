namespace BankSystem.Client.Commands
{
    using System;
    using System.Linq;
    using Data;
    using Interfaces.Core;
    using IO;

    public class DeductFeeCommand : Command
    {
        public DeductFeeCommand(string[] commandArgs, IAuthenticationManager authenticationManager)
            : base(commandArgs, authenticationManager)
        {
        }

        public override string Execute()
        {
            // DeductFee <Account number> 

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

                var checkingAccount = user
                                    .CheckingAccounts
                                    .FirstOrDefault(a => a.AccountNumber == accountNumber);

                if (checkingAccount == null)
                {
                    throw new ArgumentException(OutputMessages.InvalidAccount);
                }

                checkingAccount.DeductFee();
                currentBalance = checkingAccount.Balance;
                context.SaveChanges();
            }

            return string.Format(OutputMessages.DeductedFee, accountNumber, currentBalance);
        }
    }
}
