namespace BankSystem.Interfaces.Models
{
    public interface ICheckingAccount : IAccount
    {
        decimal Fee { get; }

        void DeductFee();
    }
}
