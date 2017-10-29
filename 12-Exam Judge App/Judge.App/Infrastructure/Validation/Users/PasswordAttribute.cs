namespace Judge.App.Infrastructure.Validation.Users
{
    using SimpleMvc.Framework.Attributes.Validation;
    using System.Linq;

    public class PasswordAttribute : PropertyValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var password = value as string;
            if (password == null)
            {
                return true;
            }

            return password.Length >= 6 &&
                   password.Any(ch => char.IsLower(ch)) &&
                   password.Any(ch => char.IsUpper(ch)) &&
                   password.Any(ch => char.IsDigit(ch));
        }
    }
}
