namespace WebServer.GameStoreApplication.Utilites
{
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public class PasswordAttribute : ValidationAttribute
    {
        public PasswordAttribute()
        {
            this.ErrorMessage = "Password should be at least 6 symbols long and contain at least 1 uppercase, 1 lowercase letter and 1 digit";
        }

        public override bool IsValid(object value)
        {
            var password = value as string;
            if (password == null)
            {
                return false;
            }

            return password.Any(s => char.IsUpper(s)) &&
                   password.Any(s => char.IsLower(s)) &&
                   password.Any(s => char.IsDigit(s));
        }
    }
}
