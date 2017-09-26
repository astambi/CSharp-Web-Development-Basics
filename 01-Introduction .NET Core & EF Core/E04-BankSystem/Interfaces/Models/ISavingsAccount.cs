namespace BankSystem.Interfaces.Models
{
    public interface ISavingsAccount : IAccount
    {
        decimal InterestRate { get; }

        void AddInterest();
    }
}
