namespace BankSystem.Core
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    using Data;
    using Interfaces.Core;
    using Interfaces.Utils;
    using IO;
    using Models;

    public class CommandProcessor : ICommandProcessor
    {
        private IAuthenticationManager authenticationManager;
        private IAccountGenerator accountGenerator;

        public CommandProcessor(IAuthenticationManager authenticationManager, IAccountGenerator accountGenerator)
        {
            this.authenticationManager = authenticationManager;
            this.accountGenerator = accountGenerator;
        }

        public string Execute(string input)
        {
            // Split input
            var commandArgs = input
                             .Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            var command = input.Length != 0 ?
                          commandArgs[0].ToLower() :
                          string.Empty;

            commandArgs = commandArgs.Skip(1).ToArray();

            string output;
            switch (command)
            {
                case "register":
                    output = this.RegisterUser(commandArgs); break;
                case "login":
                    output = this.LoginUser(commandArgs); break;
                case "logout":
                    output = this.Logout(commandArgs); break;
                case "exit":
                    output = this.Exit(commandArgs); break;
                case "add":
                    output = this.AddAccount(commandArgs); break;
                case "deposit":
                    output = this.DepositMoney(commandArgs); break;
                case "withdraw":
                    output = this.WithdrawMoney(commandArgs); break;
                case "listaccounts":
                    output = this.ListAccounts(commandArgs); break;
                case "deductfee":
                    output = this.DeductFee(commandArgs); break;
                case "addinterest":
                    output = this.AddInterest(commandArgs); break;

                default: throw new ArgumentException(OutputMessages.InvalidCommand);
            }

            return output;
        }

        private string RegisterUser(string[] commandArgs)
        {
            // Register <username> <password> <email>

            if (commandArgs.Length != 3)
            {
                throw new ArgumentException(OutputMessages.InvalidInput);
            }

            if (this.authenticationManager.IsAuthenticated())
            {
                throw new InvalidOperationException(OutputMessages.UserShouldLogOut);
            }

            var username = commandArgs[0];
            var password = commandArgs[1];
            var email = commandArgs[2];

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

        private string LoginUser(string[] commandArgs)
        {
            // Login <username> <password> 

            if (commandArgs.Length != 2)
            {
                throw new ArgumentException(OutputMessages.InvalidInput);
            }

            if (this.authenticationManager.IsAuthenticated())
            {
                throw new InvalidOperationException(OutputMessages.UserShouldLogOut);
            }

            var username = commandArgs[0];
            var password = commandArgs[1];

            using (var context = new BankSystemDbContext())
            {
                var user = context.Users
                           .FirstOrDefault(u => u.Username == username &&
                                                u.Password == password);

                if (user == null)
                {
                    throw new ArgumentException(OutputMessages.InvalidUsernameOrPassword);
                }

                this.authenticationManager.Login(user);
            }

            return string.Format(OutputMessages.UserLoggedIn, username);
        }

        private string Logout(string[] commandArgs)
        {
            // Logout

            if (!this.authenticationManager.IsAuthenticated())
            {
                throw new InvalidOperationException(OutputMessages.UserCannotLogOut);
            }

            var user = this.authenticationManager.GetCurrentUser();
            this.authenticationManager.Logout();

            return string.Format(OutputMessages.UserLoggedOut, user.Username);
        }

        private string AddAccount(string[] commandArgs)
        {
            // Add SavingAccount <initial balance> <interest rate>
            // Add CheckingAccount <initial balance> <fee>

            if (commandArgs.Length != 3)
            {
                throw new ArgumentException(OutputMessages.InvalidInput);
            }

            if (!this.authenticationManager.IsAuthenticated())
            {
                throw new InvalidOperationException(OutputMessages.UserShouldLogIn);
            }

            var accountType = commandArgs[0];
            var accountNumber = this.accountGenerator.GenerateAccountNumber();
            var balance = decimal.Parse(commandArgs[1]);
            var rateOrFee = decimal.Parse(commandArgs[2]);

            if (accountType.Equals("CheckingAccount", StringComparison.OrdinalIgnoreCase))
            {
                AddCheckingAccount(accountNumber, balance, rateOrFee);
            }
            else if (accountType.Equals("SavingsAccount", StringComparison.OrdinalIgnoreCase))
            {
                AddSavingsAccount(accountNumber, balance, rateOrFee);
            }
            else
            {
                throw new ArgumentException(string.Format(OutputMessages.InvalidAccountType, accountType));
            }

            return string.Format(OutputMessages.AccountAdded, accountNumber);
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
                var user = this.authenticationManager.GetCurrentUser();
                context.Users.Attach(user);
                savingsAccount.User = user;

                context.SavingsAccounts.Add(savingsAccount);
                context.SaveChanges();
            }
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
                var user = this.authenticationManager.GetCurrentUser();
                context.Users.Attach(user);
                checkingAccount.User = user;

                context.CheckingAccounts.Add(checkingAccount);
                context.SaveChanges();
            }
        }

        private string DepositMoney(string[] commandArgs)
        {
            // Deposit <Account number> <money> 

            if (commandArgs.Length != 2)
            {
                throw new ArgumentException(OutputMessages.InvalidInput);
            }

            if (!this.authenticationManager.IsAuthenticated())
            {
                throw new InvalidOperationException(OutputMessages.UserShouldLogIn);
            }

            var accountNumber = commandArgs[0];
            var amount = decimal.Parse(commandArgs[1]);

            if (amount <= 0)
            {
                throw new ArgumentException(OutputMessages.AmountShouldBePositive);
            }

            decimal currentBalance;

            using (var context = new BankSystemDbContext())
            {
                var user = this.authenticationManager.GetCurrentUser();
                context.Users.Attach(user);

                var savingsAccount = user
                                    .SavingsAccounts
                                    .FirstOrDefault(a => a.AccountNumber == accountNumber);

                var checkingAccount = user
                                    .CheckingAccounts
                                    .FirstOrDefault(a => a.AccountNumber == accountNumber);

                if (savingsAccount == null &&
                    checkingAccount == null)
                {
                    throw new ArgumentException(OutputMessages.InvalidAccount);
                }

                if (savingsAccount != null)
                {
                    savingsAccount.DepositMoney(amount);
                    context.SaveChanges();

                    currentBalance = savingsAccount.Balance;
                }
                else
                {
                    checkingAccount.DepositMoney(amount);
                    context.SaveChanges();

                    currentBalance = checkingAccount.Balance;
                }
            }

            return string.Format(OutputMessages.AccountHasBalance, accountNumber, currentBalance);
        }

        private string WithdrawMoney(string[] commandArgs)
        {
            // withdraw <Account number> <money> 

            if (commandArgs.Length != 2)
            {
                throw new ArgumentException(OutputMessages.InvalidInput);
            }

            if (!this.authenticationManager.IsAuthenticated())
            {
                throw new InvalidOperationException(OutputMessages.UserShouldLogIn);
            }

            var accountNumber = commandArgs[0];
            var amount = decimal.Parse(commandArgs[1]);

            if (amount <= 0)
            {
                throw new ArgumentException(OutputMessages.AmountShouldBePositive);
            }

            decimal currentBalance;

            using (var context = new BankSystemDbContext())
            {
                var user = this.authenticationManager.GetCurrentUser();
                context.Users.Attach(user);

                var savingsAccount = user
                                    .SavingsAccounts
                                    .FirstOrDefault(a => a.AccountNumber == accountNumber);

                var checkingAccount = user
                                    .CheckingAccounts
                                    .FirstOrDefault(a => a.AccountNumber == accountNumber);

                if (savingsAccount == null &&
                    checkingAccount == null)
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

        private string ListAccounts(string[] commandArgs)
        {
            // ListAccounts

            if (!this.authenticationManager.IsAuthenticated())
            {
                throw new InvalidOperationException(OutputMessages.UserShouldLogIn);
            }

            var builder = new StringBuilder();

            using (var context = new BankSystemDbContext())
            {
                var user = this.authenticationManager.GetCurrentUser();
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

        private string DeductFee(string[] commandArgs)
        {
            // DeductFee <Account number> 

            if (commandArgs.Length != 1)
            {
                throw new ArgumentException(OutputMessages.InvalidInput);
            }

            if (!this.authenticationManager.IsAuthenticated())
            {
                throw new InvalidOperationException(OutputMessages.UserShouldLogIn);
            }

            var accountNumber = commandArgs[0];

            decimal currentBalance;

            using (var context = new BankSystemDbContext())
            {
                var user = this.authenticationManager.GetCurrentUser();
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

        private string AddInterest(string[] input)
        {
            // AddInterest <Account number> 

            if (input.Length != 1)
            {
                throw new ArgumentException(OutputMessages.InvalidInput);
            }

            if (!this.authenticationManager.IsAuthenticated())
            {
                throw new InvalidOperationException(OutputMessages.UserShouldLogIn);
            }

            var accountNumber = input[0];

            decimal currentBalance;

            using (var context = new BankSystemDbContext())
            {
                var user = this.authenticationManager.GetCurrentUser();
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

        private string Exit(string[] commandArgs)
        {
            Environment.Exit(0);

            return string.Empty;
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
    }
}
