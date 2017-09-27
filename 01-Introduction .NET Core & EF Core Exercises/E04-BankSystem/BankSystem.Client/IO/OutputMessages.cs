namespace BankSystem.Client.IO
{
    public class OutputMessages
    {
        public const string AccountAdded = "Succesfully added account with number {0}";
        public const string AccountBalance = "--Account {0} {1:f2}";
        public const string AccountBalanceInvalid = "Account balance cannot be negative";
        public const string AccountCannotBeNull = "Account cannot be null";
        public const string AccountFeeInvalid = "Account fee cannot be negative";
        public const string AccountHasBalance = "Account {0} has balance of {1:f2}";
        public const string AccountInterestRateInvalid = "Account interest rate cannot be negative, even though it is right now ;)";
        public const string AccountLengthInvalid = "Account number length should be exactly 10 symbols";

        public const string AddedInterest = "Added interest to {0}. Current balance: {1:f2}";
        public const string AmountShouldBePositive = "Amount should be positive";

        public const string DeductedFee = "Deducted fee from {0}. Current balance: {1:f2}";

        public const string InvalidAccount = "Account does not exist";
        public const string InvalidAccountType = "Invalid account type {0}";
        public const string InvalidCommand = "Command not supported";
        public const string InvalidEmail = "Incorrent email";
        public const string InvalidInput = "Invalid input";
        public const string InvalidPassword = "Incorrect password";
        public const string InvalidUsername = "Incorrect username";
        public const string InvalidUsernameOrPassword = "Incorrect username / password";

        public const string EmailTaken = "Email already taken";

        public const string UserCannotBeNull = "User cannot be null";
        public const string UserCannotLogOut = "Cannot log out. No user was logged in.";
        public const string UserLoggedIn = "Succesfully logged in {0}";
        public const string UserLoggedOut = "User {0} successfully logged out";
        public const string UserRegistered = "{0} was registered in the system";
        public const string UserShouldLogIn = "User should log in first";
        public const string UserShouldLogOut = "Current user should log out first";

        public const string UsernameTaken = "Username already taken";
    }
}
