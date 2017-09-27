namespace BankSystem.Client.Commands
{
    using System;
    using Data;
    using Interfaces.Core;
    using IO;
    using Models;

    public class AddCommand : Command
    {
        public AddCommand(string[] commandArgs, IAuthenticationManager authenticationManager)
            : base(commandArgs, authenticationManager)
        {
        }

        public override string Execute()
        {
            // Add SavingAccount <initial balance> <interest rate>
            // Add CheckingAccount <initial balance> <fee>

            if (this.CommandArgs.Length != 3)
            {
                throw new ArgumentException(OutputMessages.InvalidInput);
            }

            if (!this.AuthenticationManager.IsAuthenticated())
            {
                throw new InvalidOperationException(OutputMessages.UserShouldLogIn);
            }

            var accountType = this.CommandArgs[0];
            var accountNumber = this.GenerateAccountNumber();
            var balance = decimal.Parse(this.CommandArgs[1]);
            var rateOrFee = decimal.Parse(this.CommandArgs[2]);

            if (accountType.Equals("CheckingAccount", StringComparison.OrdinalIgnoreCase))
            {
                this.AddCheckingAccount(accountNumber, balance, rateOrFee);
            }
            else if (accountType.Equals("SavingsAccount", StringComparison.OrdinalIgnoreCase))
            {
                this.AddSavingsAccount(accountNumber, balance, rateOrFee);
            }
            else
            {
                throw new ArgumentException(string.Format(OutputMessages.InvalidAccountType, accountType));
            }

            return string.Format(OutputMessages.AccountAdded, accountNumber);
        }

        private void AddCheckingAccount(string accountNumber, decimal balance, decimal rateOrFee)
        {
            var checkingAccount = new CheckingAccount()
            {
                AccountNumber = accountNumber,
                Balance = balance,
                Fee = rateOrFee
            };

            this.ValidateCheckingAccount(checkingAccount);

            using (var context = new BankSystemDbContext())
            {
                var user = this.AuthenticationManager.GetCurrentUser();
                context.Users.Attach(user);

                checkingAccount.User = user;

                context.CheckingAccounts.Add(checkingAccount);
                context.SaveChanges();
            }
        }

        private void AddSavingsAccount(string accountNumber, decimal balance, decimal rateOrFee)
        {
            var savingsAccount = new SavingsAccount()
            {
                AccountNumber = accountNumber,
                Balance = balance,
                InterestRate = rateOrFee
            };

            this.ValidateSavingsAccount(savingsAccount);

            using (var context = new BankSystemDbContext())
            {
                var user = this.AuthenticationManager.GetCurrentUser();
                context.Users.Attach(user);

                savingsAccount.User = user;

                context.SavingsAccounts.Add(savingsAccount);
                context.SaveChanges();
            }
        }

        private void ValidateCheckingAccount(CheckingAccount account)
        {
            bool isValid = true;
            string errors = string.Empty;

            if (account == null)
            {
                errors = OutputMessages.AccountCannotBeNull;
                throw new ArgumentException(errors);
            }

            if (account.AccountNumber.Length != 10)
            {
                isValid = false;
                errors += OutputMessages.AccountLengthInvalid + Environment.NewLine;
            }

            if (account.Balance < 0)
            {
                isValid = false;
                errors += OutputMessages.AccountBalanceInvalid + Environment.NewLine;
            }

            if (account.Fee < 0)
            {
                isValid = false;
                errors += OutputMessages.AccountFeeInvalid + Environment.NewLine;
            }

            if (!isValid)
            {
                throw new ArgumentException(errors.Trim());
            }
        }

        private void ValidateSavingsAccount(SavingsAccount account)
        {
            bool isValid = true;
            string errors = string.Empty;

            if (account == null)
            {
                errors = OutputMessages.AccountCannotBeNull;
                throw new ArgumentException(errors);
            }

            if (account.AccountNumber.Length != 10)
            {
                isValid = false;
                errors += OutputMessages.AccountLengthInvalid + Environment.NewLine;
            }

            if (account.Balance < 0)
            {
                isValid = false;
                errors += OutputMessages.AccountBalanceInvalid + Environment.NewLine;
            }

            if (account.InterestRate < 0)
            {
                isValid = false;
                errors += OutputMessages.AccountInterestRateInvalid + Environment.NewLine;
            }

            if (!isValid)
            {
                throw new ArgumentException(errors.Trim());
            }
        }

        private string GenerateAccountNumber()
        {
            return Guid.NewGuid()
                  .ToString()
                  .Replace("-", string.Empty)
                  .Substring(0, 10)
                  .ToUpper();
        }
    }
}
