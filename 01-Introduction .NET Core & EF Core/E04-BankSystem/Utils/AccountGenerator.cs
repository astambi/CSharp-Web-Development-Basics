namespace BankSystem.Utils
{
    using System;
    using Interfaces.Utils;

    public class AccountGenerator : IAccountGenerator
    {
        public string GenerateAccountNumber()
        {
            return Guid.NewGuid()
                  .ToString()
                  .Replace("-", string.Empty)
                  .Substring(0, 10)
                  .ToUpper();
        }
    }
}
