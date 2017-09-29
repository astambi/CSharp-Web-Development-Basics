namespace BankSystem.Models.Attributes
{
    using System;
    using System.Text.RegularExpressions;

    [AttributeUsage(AttributeTargets.Property)]
    public class ValidUsernameAttribute : ValidEmailAttribute
    {
        public override bool IsValid(object value)
        {
            var valueAsString = (string)value;
            
            var pattern = @"^[A-Za-z][A-Za-z0-9]{2,}$";

            if (!Regex.IsMatch(valueAsString, pattern))
            {
                this.ErrorMessage = "Invalid username!";
                return false;
            }

            return true;
        }
    }
}