namespace BankSystem.Interfaces.Models
{
    using System.Collections.Generic;

    public interface IUser
    {
        int Id { get; }

        string Username { get; }

        string Password { get; }

        string Email { get; }

        ICollection<ISavingsAccount> SavingsAccounts { get; set; }

        ICollection<ICheckingAccount> CheckingAccounts { get; set; }
    }
}
