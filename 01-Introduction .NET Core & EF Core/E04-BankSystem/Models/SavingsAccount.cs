namespace BankSystem.Models
{
    public class SavingsAccount : Account
    {
        public decimal InterestRate { get; set; }

        public void AddInterest()
        {
            this.DepositMoney(this.Balance * this.InterestRate);
        }
    }
}
