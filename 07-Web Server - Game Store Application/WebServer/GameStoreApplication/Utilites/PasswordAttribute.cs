namespace WebServer.GameStoreApplication.Utilites
{
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public class EmailAttribute : ValidationAttribute
    {
        public EmailAttribute()
        {
            this.ErrorMessage = "Email should contain @ sign and a period.";
        }

        public override bool IsValid(object value)
        {
            var email = value as string;
            if (email == null)
            {
                return false;
            }

            return email.Contains('@') &&
                   email.Contains('.');
        }
    }
}
