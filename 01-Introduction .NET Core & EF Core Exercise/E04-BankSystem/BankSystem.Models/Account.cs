namespace BankSystem.Models
{
    public abstract class Account
    {
        public int Id { get; set; }

        public string AccountNumber { get; set; }

        public decimal Balance { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public void DepositMoney(decimal value)
        {
            this.Balance += value;
        }

        public void WithdrawMoney(decimal value)
        {
            this.Balance -= value;
        }
    }
}
