namespace BankSystem.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Attributes;

    public class User
    {
        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        [ValidUsername]
        public string Username { get; set; }

        [Required]
        [MinLength(6)]
        [ValidPassword]
        public string Password { get; set; }

        [Required]
        [ValidEmail]
        public string Email { get; set; }

        public ICollection<SavingsAccount> SavingsAccounts { get; set; } = new List<SavingsAccount>();

        public ICollection<CheckingAccount> CheckingAccounts { get; set; } = new List<CheckingAccount>();
    }
}
