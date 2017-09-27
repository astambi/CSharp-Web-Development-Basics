namespace BankSystem.Models.Attributes
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Text.RegularExpressions;

    [AttributeUsage(AttributeTargets.Property)]
    public class ValidPasswordAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var valueAsString = (string)value;
            var length = valueAsString.Trim().Length;

            if (length < 6)
            {
                this.ErrorMessage = "Invalid password length!";
                return false;
            }

            if (!Regex.IsMatch(valueAsString, @"[A-Z]+"))
            {
                this.ErrorMessage = "Password does not contain an uppercase letter!";
                return false;
            }

            if (!Regex.IsMatch(valueAsString, @"[a-z]+"))
            {
                this.ErrorMessage = "Password does not contain a lowercase letter!";
                return false;
            }

            if (!Regex.IsMatch(valueAsString, @"\d+"))
            {
                this.ErrorMessage = "Password does not contain a digit!";
                return false;
            }

            return true;
        }
    }
}
