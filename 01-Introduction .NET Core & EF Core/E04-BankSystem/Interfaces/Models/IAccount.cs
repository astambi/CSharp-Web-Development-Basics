namespace BankSystem.Interfaces.Models
{
    public interface IAccount
    {
        int Id { get; }

        string AccountNumber { get; }

        decimal Balance { get; }

        int UserId { get; }

        IUser User { get; }

        void DepositMoney(decimal value);

        void WithdrawMoney(decimal value);
    }
}
