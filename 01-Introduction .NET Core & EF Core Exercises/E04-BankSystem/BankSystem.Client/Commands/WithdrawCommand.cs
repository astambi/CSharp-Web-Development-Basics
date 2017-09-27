namespace BankSystem.Client.Commands
{
    using System;
    using System.Linq;
    using Data;
    using Interfaces.Core;
    using IO;

    public class WithdrawCommand : Command
    {
        public WithdrawCommand(string[] commandArgs, IAuthenticationManager authenticationManager)
            : base(commandArgs, authenticationManager)
        {
        }

        public override string Execute()
        {
            // withdraw <Account number> <money> 

            if (this.CommandArgs.Length != 2)
            {
                throw new ArgumentException(OutputMessages.InvalidInput);
            }

            if (!this.AuthenticationManager.IsAuthenticated())
            {
                throw new InvalidOperationException(OutputMessages.UserShouldLogIn);
            }

            var accountNumber = this.CommandArgs[0];
            var amount = decimal.Parse(this.CommandArgs[1]);

            if (amount <= 0)
            {
                throw new ArgumentException(OutputMessages.AmountShouldBePositive);
            }

            decimal currentBalance;

            using (var context = new BankSystemDbContext())
            {
                var user = this.AuthenticationManager.GetCurrentUser();
                context.Users.Attach(user);

                var savingsAccount = user
                                    .SavingsAccounts
                                    .FirstOrDefault(a => a.AccountNumber == accountNumber);

                var checkingAccount = user
                                    .CheckingAccounts
                                    .FirstOrDefault(a => a.AccountNumber == accountNumber);

                if (savingsAccount == null && checkingAccount == null)
                {
                    throw new ArgumentException(OutputMessages.InvalidAccount);
                }

                if (savingsAccount != null)
                {
                    savingsAccount.WithdrawMoney(amount);
                    context.SaveChanges();

                    currentBalance = savingsAccount.Balance;
                }
                else
                {
                    checkingAccount.WithdrawMoney(amount);
                    context.SaveChanges();

                    currentBalance = checkingAccount.Balance;
                }
            }

            return string.Format(OutputMessages.AccountHasBalance, accountNumber, currentBalance);
        }
    }
}
