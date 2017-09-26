namespace BankSystem.Models
{
    public class CheckingAccount : Account
    {
        public decimal Fee { get; set; }

        public void DeductFee()
        {
            this.WithdrawMoney(this.Fee);
        }
    }
}
